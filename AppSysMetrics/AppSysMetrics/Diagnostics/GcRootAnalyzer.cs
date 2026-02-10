using System.Diagnostics;
using System.Reflection;
using AppSysMetrics.Diagnostics.Models;
using Microsoft.Diagnostics.Runtime;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppSysMetrics.Diagnostics;

/// <summary>
/// Analyzes GC roots for specific leak-suspect types on a live <see cref="ClrHeap"/>.
/// Called by <see cref="ClrMdHeapAnalyzer"/> within the DataTarget snapshot scope so that
/// root enumeration and heap object enumeration share the same process snapshot.
///
/// Uses a reverse-reference algorithm:
///   1. Build a GC root address set.
///   2. Single heap pass: collect target instances + build lightweight parent map (addresses only).
///   3. Walk backward from each target instance toward a GC root.
///   4. Resolve field names only for objects on the actual retention path.
///   5. Produce human-readable retention paths showing every type in the chain.
///
/// Performance-critical design decisions:
///   - Phase 2 does NOT cache objectTypes for every heap object. Type names are resolved
///     lazily during the backward walk using heap.GetObjectType(), which is a cheap O(1)
///     metadata lookup in ClrMD (no heap traversal required).
///   - ScoreInstancesByUserCode caps the number of instances scored to avoid pathological
///     cases like Byte[] with 20,000+ instances, and uses stride-based sampling to cover
///     the full instance list (recently-allocated objects tend to be at the end).
///   - Per-type and global timeouts enforce hard bounds on analysis time.
///
/// Stateless, pure computation — similar in philosophy to <see cref="DumpDiffService"/>.
/// </summary>
public sealed class GcRootAnalyzer
{
    private readonly DumpAnalyzerOptions _options;
    private readonly ILogger<GcRootAnalyzer> _logger;

    /// <summary>
    /// Cached set of assembly name prefixes that represent user/application code.
    /// Built once via <see cref="BuildUserAssemblyPrefixes"/> on first analysis, then reused.
    /// </summary>
    private HashSet<string>? _userAssemblyPrefixes;

    /// <summary>Maximum instances to score per type for "Just My Code" prioritization.</summary>
    private const int MaxInstancesToScore = 500;

    /// <summary>Maximum instances to sample per type for root walking.</summary>
    private const int MaxInstancesToSample = 50;

    public GcRootAnalyzer(
        IOptions<DumpAnalyzerOptions> options,
        ILogger<GcRootAnalyzer> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// Analyzes GC roots for the specified target types on the given heap.
    /// Must be called while the DataTarget snapshot is still alive.
    /// </summary>
    public RootAnalysisResult AnalyzeRoots(
        ClrHeap heap,
        IReadOnlyList<string> targetTypeNames)
    {
        var globalSw = Stopwatch.StartNew();
        var targetSet = new HashSet<string>(targetTypeNames);
        var wasTimedOut = false;
        var skippedTypes = new List<string>();

        // Build the user assembly prefix set once (cached for process lifetime)
        _userAssemblyPrefixes ??= BuildUserAssemblyPrefixes();

        _logger.LogInformation(
            "Starting reverse-reference root analysis for {Count} target types: {Types}",
            targetTypeNames.Count,
            string.Join(", ", targetTypeNames));

        // ── Phase 1: Build root address set ──
        var rootAddressMap = new Dictionary<ulong, (string Kind, string TypeName)>();
        foreach (var root in heap.EnumerateRoots())
        {
            if (!root.Object.IsValid)
                continue;

            rootAddressMap.TryAdd(root.Object.Address, (
                root.RootKind.ToString(),
                root.Object.Type?.Name ?? "UNKNOWN"));
        }

        _logger.LogInformation(
            "Phase 1 complete: {RootCount} GC root addresses indexed in {Elapsed}ms",
            rootAddressMap.Count,
            globalSw.ElapsedMilliseconds);

        if (globalSw.Elapsed >= _options.RootAnalysisGlobalTimeout)
        {
            wasTimedOut = true;
            return BuildResult(targetTypeNames, [], skippedTypes, globalSw, wasTimedOut);
        }

        // ── Phase 2: Single heap pass — collect target instances + build lightweight parent map ──
        //
        // parentMap: childAddress → parentAddress (addresses only — fast)
        // We use EnumerateReferenceAddresses which is much cheaper than EnumerateReferencesWithFields.
        // Field names are resolved later, only for objects on the actual retention path.
        //
        // IMPORTANT: We do NOT build an objectTypes dictionary here. That previously stored
        // a type name for every single object on the heap (~270K entries) which was very expensive.
        // Type names are resolved lazily via heap.GetObjectType() during backward walks — this is
        // a cheap metadata lookup in ClrMD.

        var targetInstances = new Dictionary<string, List<ulong>>();
        foreach (var t in targetTypeNames)
            targetInstances[t] = [];

        var parentMap = new Dictionary<ulong, ulong>();  // childAddr → parentAddr

        var objectCount = 0;
        foreach (var obj in heap.EnumerateObjects())
        {
            if (globalSw.Elapsed >= _options.RootAnalysisGlobalTimeout)
            {
                wasTimedOut = true;
                _logger.LogWarning("Root analysis hit global timeout during heap enumeration");
                break;
            }

            if (!obj.IsValid || obj.Type is null)
                continue;

            objectCount++;
            var typeName = obj.Type.Name;
            if (typeName is null)
                continue;

            // Collect target instances
            if (targetSet.Contains(typeName))
                targetInstances[typeName].Add(obj.Address);

            // Index outgoing references for ALL objects (including target types).
            // Previously we skipped reference indexing for target-type objects with `continue`,
            // but this left gaps in the parentMap. For example, List<byte[]> references Byte[][]
            // via _items, and if no other object claims Byte[][] first, the parentMap entry
            // is correctly set. But target-type objects may also be containers (e.g. String
            // referencing Char[]) and skipping them broke parent chains.
            //
            // User-code parent preference: when the current object is a user-code type, it
            // overwrites any existing framework parent in the map. This solves the problem
            // where transient framework objects (e.g. List<T>+Enumerator) claim parenthood
            // via TryAdd before the actual owner (e.g. MemoryLeakService) is enumerated.
            var currentIsUserCode = IsUserCodeType(typeName);
            try
            {
                foreach (var childAddr in obj.EnumerateReferenceAddresses(
                    carefully: true, considerDependantHandles: false))
                {
                    if (!parentMap.TryAdd(childAddr, obj.Address) && currentIsUserCode)
                    {
                        // Slot already taken — overwrite if existing parent is framework code
                        var existingParent = parentMap[childAddr];
                        var existingType = GetTypeName(heap, existingParent);
                        if (!IsUserCodeType(existingType))
                        {
                            parentMap[childAddr] = obj.Address;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Corrupted object — skip silently
            }
        }

        _logger.LogInformation(
            "Phase 2 complete in {Elapsed}ms: {ObjCount} objects, {ParentMapSize} parent entries, target instances: {TargetCounts}",
            globalSw.ElapsedMilliseconds,
            objectCount,
            parentMap.Count,
            string.Join(", ", targetInstances.Select(kv => $"{kv.Key}={kv.Value.Count}")));

        if (globalSw.Elapsed >= _options.RootAnalysisGlobalTimeout)
        {
            wasTimedOut = true;
            return BuildResult(targetTypeNames, [], skippedTypes, globalSw, wasTimedOut);
        }

        // ── Phase 3: Walk backward from target instances to roots ──
        // "Just My Code" prioritization: score a capped sample of instances by whether their
        // parent chain passes through user/application code vs. framework-only code.
        var typeAnalyses = new List<TypeRootAnalysis>();

        foreach (var typeName in targetTypeNames)
        {
            if (globalSw.Elapsed >= _options.RootAnalysisGlobalTimeout)
            {
                wasTimedOut = true;
                skippedTypes.Add(typeName);
                continue;
            }

            var instances = targetInstances[typeName];
            if (instances.Count == 0)
            {
                _logger.LogInformation("Type {TypeName}: 0 instances found on heap, skipping", typeName);
                skippedTypes.Add(typeName);
                continue;
            }

            var typeSw = Stopwatch.StartNew();
            var hits = new List<RootHit>();

            _logger.LogInformation(
                "Phase 3: Analyzing {TypeName} — {Count} instances, scoring up to {MaxScore}, sampling up to {MaxSample}",
                typeName, instances.Count, MaxInstancesToScore, MaxInstancesToSample);

            // Score and sort instances: "Just My Code" first, with caps
            var scored = ScoreInstancesByUserCode(instances, parentMap, heap, globalSw);

            if (globalSw.Elapsed >= _options.RootAnalysisGlobalTimeout)
            {
                wasTimedOut = true;
                _logger.LogWarning("Root analysis hit global timeout during scoring for {TypeName}", typeName);
                skippedTypes.Add(typeName);
                continue;
            }

            var sampleSize = Math.Min(scored.Count, MaxInstancesToSample);

            foreach (var targetAddr in scored.Take(sampleSize))
            {
                if (globalSw.Elapsed >= _options.RootAnalysisGlobalTimeout)
                {
                    wasTimedOut = true;
                    break;
                }

                if (typeSw.Elapsed >= _options.RootAnalysisPerTypeTimeout)
                {
                    _logger.LogWarning("Per-type timeout for {TypeName} at {Elapsed}ms", typeName, typeSw.ElapsedMilliseconds);
                    break;
                }

                var chain = WalkBackwardToRoot(
                    targetAddr, typeName, parentMap, rootAddressMap, heap);

                if (chain is not null)
                    hits.Add(chain.Value);
            }

            // Group by full retention path so that distinct chains remain separate.
            // Previously grouped by (RootKind, RootObjectTypeName) which merged user-code paths
            // with framework paths that happen to share the same root kind/type.
            // Sort: paths containing user code first, then by retained count.
            var grouped = hits
                .GroupBy(h => h.RetentionPath ?? "(direct)")
                .Select(g =>
                {
                    var representative = g.First();
                    return new GcRootInfo
                    {
                        RootKind = representative.RootKind,
                        RootAddress = representative.RootAddress,
                        RootObjectTypeName = representative.RootObjectTypeName,
                        RetentionPath = representative.RetentionPath,
                        RetainedInstanceCount = g.Sum(h => h.Count),
                        // Only count as "user code" if the user-code type is a direct owner
                        // (within a few hops), not incidentally in the chain through framework plumbing.
                        HasUserCode = g.Any(h => h.HasUserCode
                            && h.UserCodeDepth <= MaxDirectOwnershipDepth)
                    };
                })
                .OrderByDescending(r => r.HasUserCode) // User-code paths first
                .ThenByDescending(r => r.RetainedInstanceCount)
                .ToList();

            var userCodeGroups = grouped.Count(g => g.HasUserCode);

            // When user-code paths exist, show only those — framework-only paths are noise.
            // When no user-code paths exist, the type is excluded entirely (below).
            var relevantRoots = userCodeGroups > 0
                ? grouped.Where(g => g.HasUserCode).ToList()
                : grouped;

            var wasTruncated = relevantRoots.Count > _options.MaxRootsPerType;
            var topRoots = relevantRoots.Take(_options.MaxRootsPerType).ToList();

            typeSw.Stop();

            _logger.LogInformation(
                "Type {TypeName}: {HitCount} root hits → {GroupCount} groups ({UserCodeGroups} with user code), top {TopCount} reported in {Elapsed}ms",
                typeName, hits.Count, grouped.Count, userCodeGroups, topRoots.Count, typeSw.ElapsedMilliseconds);

            // Only include types that have at least one retention path through user code.
            // Types with framework-only paths (e.g. Char[] via SharedArrayPool, StringBuilder
            // chains) are noise — not actionable for the developer.
            if (userCodeGroups == 0)
            {
                _logger.LogInformation(
                    "Type {TypeName}: excluded from results — no retention paths through user code",
                    typeName);
                continue;
            }

            typeAnalyses.Add(new TypeRootAnalysis
            {
                TypeName = typeName,
                TotalRootCount = grouped.Count,
                Roots = topRoots,
                WasTruncated = wasTruncated,
                AnalysisDuration = typeSw.Elapsed
            });
        }

        return BuildResult(targetTypeNames, typeAnalyses, skippedTypes, globalSw, wasTimedOut);
    }

    /// <summary>
    /// Walks backward from a target instance through the parent map until we reach either:
    ///   (a) a user-code type (the primary goal — answers "which of MY types holds this?"), or
    ///   (b) a GC root (fallback if no user-code type is found in the chain).
    /// The walk stops at whichever comes first, keeping paths short and developer-focused.
    ///
    /// Resolves field names only for objects on the actual retention path (expensive but targeted).
    /// Returns a RootHit with the retention path, or null if no meaningful chain was found.
    /// </summary>
    private RootHit? WalkBackwardToRoot(
        ulong targetAddr,
        string targetTypeName,
        Dictionary<ulong, ulong> parentMap,
        Dictionary<ulong, (string Kind, string TypeName)> rootAddressMap,
        ClrHeap heap)
    {
        // Walk backward collecting the address chain (fast, address-only)
        var chain = new List<ulong> { targetAddr };
        var visited = new HashSet<ulong> { targetAddr };
        var maxDepth = _options.MaxRetentionPathDepth;
        var foundRoot = false;
        var foundUserCode = false;
        var userCodeDepth = -1;
        string rootKind = "Unknown";
        string rootTypeName = "?";

        var currentAddr = targetAddr;
        for (var depth = 0; depth < maxDepth; depth++)
        {
            // Check if current is a GC root
            if (rootAddressMap.TryGetValue(currentAddr, out var rootInfo))
            {
                foundRoot = true;
                rootKind = rootInfo.Kind;
                rootTypeName = rootInfo.TypeName;
                break;
            }

            // Walk to parent
            if (!parentMap.TryGetValue(currentAddr, out var parentAddr))
                break;

            if (!visited.Add(parentAddr))
                break;

            chain.Add(parentAddr);
            currentAddr = parentAddr;

            // Check if the parent is a user-code type — if so, we've found the answer.
            // depth is 0-based from the target, so depth+1 = number of hops from target.
            var parentTypeName = GetTypeName(heap, parentAddr);
            if (IsUserCodeType(parentTypeName))
            {
                foundUserCode = true;
                userCodeDepth = depth + 1; // hops from target to this user-code type
                // The user-code type is the effective root from the developer's perspective.
                rootKind = "UserCode";
                rootTypeName = parentTypeName;
                break;
            }
        }

        // Validate direct ownership: when user code is found at depth ≥ 2, check whether
        // the path from user-code type to target passes through framework infrastructure.
        // Example:  LeakLab → _renderHandle:EndpointHtmlRenderer → ... → Int32[]
        //   The immediate child of LeakLab is EndpointHtmlRenderer (Microsoft.*)  → framework plumbing.
        // Contrast: MemoryLeakService → _leakedBlobs:List<Byte[]> → _items:Byte[][] → Byte[]
        //   The immediate child is List<Byte[]> (System.*)  → user's own data structure.
        // When the first hop from user code enters Microsoft.*/Internal.* framework types,
        // the user-code type is just a Blazor component or similar that indirectly holds
        // the target through framework infrastructure — not a deliberate allocation.
        if (foundUserCode && userCodeDepth >= 2)
        {
            // chain (before reversal): [targetAddr, ..., intermediateAddr, userCodeAddr]
            // The immediate child of user code is at chain[chain.Count - 2]
            var immediateChildAddr = chain[chain.Count - 2];
            var immediateChildType = GetTypeName(heap, immediateChildAddr);
            if (IsFrameworkInfrastructureType(immediateChildType))
            {
                foundUserCode = false;
                userCodeDepth = -1;
            }
        }

        if (chain.Count <= 1 && !foundRoot)
            return null;

        // Build the readable path (reverse so it reads top → ... → target)
        // Resolve field names for each parent→child hop.
        chain.Reverse(); // Now: top-of-chain → ... → target

        var pathParts = new List<string>();

        // First element is the top of chain (user-code type, GC root, or highest reached)
        var topType = foundRoot
            ? rootTypeName
            : GetTypeName(heap, chain[0]);
        pathParts.Add(ShortenTypeName(topType));

        // For each hop, resolve the field name from parent → child
        for (var i = 0; i < chain.Count - 1; i++)
        {
            var parentAddr = chain[i];
            var childAddr = chain[i + 1];
            var childType = i == chain.Count - 2
                ? targetTypeName
                : GetTypeName(heap, childAddr);

            var fieldName = ResolveFieldName(heap, parentAddr, childAddr);
            pathParts.Add($"{fieldName}:{ShortenTypeName(childType)}");
        }

        var path = string.Join(" → ", pathParts);

        return new RootHit(
            rootKind,
            rootTypeName,
            chain[0],
            path,
            1,
            foundUserCode,
            userCodeDepth);
    }

    /// <summary>
    /// Scores a stratified sample of target instances by whether their parent chain passes through
    /// user/application code. Instances retained by user code are prioritized (sorted first).
    /// This is the "Just My Code" filter — uses only cheap address lookups + heap.GetObjectType().
    ///
    /// Uses stride-based sampling to cover the full instance list evenly. Instances are enumerated
    /// in heap address order — recently allocated objects (like our leaked blobs) tend to be near
    /// the end. Taking only the first N would miss them; stride sampling ensures full coverage.
    ///
    /// For types with many instances (e.g. Byte[] with 20,000+ instances), only
    /// <see cref="MaxInstancesToScore"/> are scored. The rest are not included.
    /// </summary>
    private List<ulong> ScoreInstancesByUserCode(
        List<ulong> instances,
        Dictionary<ulong, ulong> parentMap,
        ClrHeap heap,
        Stopwatch globalSw)
    {
        // Build a stratified sample: pick instances evenly across the full list
        var scoreCap = Math.Min(instances.Count, MaxInstancesToScore);
        var stride = instances.Count <= scoreCap ? 1 : (double)instances.Count / scoreCap;

        var scored = new List<(ulong Addr, int UserCodeScore)>(scoreCap);
        var maxProbe = Math.Min(_options.MaxRetentionPathDepth, 10); // Don't walk too deep for scoring
        var userCodeHits = 0;

        for (var i = 0; i < scoreCap; i++)
        {
            // Bail on global timeout
            if (globalSw.Elapsed >= _options.RootAnalysisGlobalTimeout)
                break;

            // Stride-based index: evenly spaced across the full instance list
            var idx = (int)(i * stride);
            if (idx >= instances.Count) idx = instances.Count - 1;

            var addr = instances[idx];
            var score = 0;
            var current = addr;
            var visited = new HashSet<ulong> { addr };

            for (var depth = 0; depth < maxProbe; depth++)
            {
                if (!parentMap.TryGetValue(current, out var parentAddr))
                    break;
                if (!visited.Add(parentAddr))
                    break;

                // Lazy type resolution: heap.GetObjectType() is a cheap metadata lookup
                var parentType = heap.GetObjectType(parentAddr);
                if (parentType?.Name is not null && IsUserCodeType(parentType.Name))
                {
                    score += 10; // User code in chain — high priority
                }

                current = parentAddr;
            }

            scored.Add((addr, score));
            if (score > 0) userCodeHits++;
        }

        // Sort scored instances by score descending — user-code-retained first
        scored.Sort((a, b) => b.UserCodeScore.CompareTo(a.UserCodeScore));

        _logger.LogInformation(
            "Scored {Scored}/{Total} instances (stride={Stride:F1}): {UserCodeHits} with user-code in chain, top score={TopScore}",
            scored.Count, instances.Count, stride,
            userCodeHits,
            scored.Count > 0 ? scored[0].UserCodeScore : 0);

        return scored.Select(s => s.Addr).ToList();
    }

    /// <summary>
    /// Discovers user/application assembly name prefixes using a two-tier strategy:
    /// <list type="number">
    ///   <item>
    ///     <b>Explicit mode</b>: If any loaded assembly is annotated with
    ///     <see cref="AnalyzeMemoryLeaksAttribute"/>, only those assemblies are considered user code.
    ///   </item>
    ///   <item>
    ///     <b>Auto mode</b>: Otherwise, all non-framework, non-dynamic assemblies are included.
    ///   </item>
    /// </list>
    /// Called once and cached for the process lifetime.
    /// </summary>
    private HashSet<string> BuildUserAssemblyPrefixes()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        // The diagnostics library itself should never be considered user code —
        // the analysis tool must not report its own types as leak suspects.
        var selfAssemblyName = typeof(GcRootAnalyzer).Assembly.GetName().Name;

        // Tier 1: Check for explicit [AnalyzeMemoryLeaks] attribute
        var explicit_ = assemblies
            .Where(a => a.GetCustomAttribute<AnalyzeMemoryLeaksAttribute>() is not null)
            .Select(a => a.GetName().Name!)
            .Where(n => n is not null && n != selfAssemblyName)
            .ToHashSet();

        if (explicit_.Count > 0)
        {
            _logger.LogInformation(
                "User code detection (explicit mode): {Count} assemblies marked with [AnalyzeMemoryLeaks]: {Assemblies}",
                explicit_.Count, string.Join(", ", explicit_));
            return explicit_;
        }

        // Tier 2: Auto-discover by excluding known framework assembly name prefixes
        string[] frameworkPrefixes = ["System", "Microsoft", "Internal", "Interop",
                                      "Newtonsoft", "netstandard", "mscorlib", "WindowsBase"];

        var auto = assemblies
            .Where(a => !a.IsDynamic)
            .Select(a => a.GetName().Name)
            .OfType<string>()
            .Where(name => name != selfAssemblyName)
            .Where(name => !frameworkPrefixes.Any(fp => name.StartsWith(fp, StringComparison.Ordinal)))
            .ToHashSet();

        _logger.LogInformation(
            "User code detection (auto mode): {Count} user assemblies discovered: {Assemblies}",
            auto.Count, string.Join(", ", auto));

        return auto;
    }

    /// <summary>
    /// Returns true if a fully-qualified type name belongs to a user/application assembly.
    /// Initializes the assembly prefix cache on first call if needed.
    /// </summary>
    public bool IsUserCode(string typeName)
    {
        _userAssemblyPrefixes ??= BuildUserAssemblyPrefixes();
        return IsUserCodeType(typeName);
    }

    private bool IsUserCodeType(string typeName)
    {
        foreach (var prefix in _userAssemblyPrefixes!)
        {
            if (typeName.StartsWith(prefix, StringComparison.Ordinal)
                && typeName.Length > prefix.Length
                && typeName[prefix.Length] == '.')
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns true if a type is framework infrastructure — Microsoft.*, Internal.*, Interop.* —
    /// as opposed to System.* container types or user code. Used to detect when a retention
    /// path from user code to a target object passes through framework plumbing (e.g. Blazor's
    /// EndpointHtmlRenderer) rather than the user's own data structures.
    /// </summary>
    private static bool IsFrameworkInfrastructureType(string typeName)
    {
        return typeName.StartsWith("Microsoft.", StringComparison.Ordinal)
            || typeName.StartsWith("Internal.", StringComparison.Ordinal)
            || typeName.StartsWith("Interop.", StringComparison.Ordinal);
    }

    /// <summary>
    /// Resolves the field name (or array indicator) for a specific parent → child reference.
    /// This is the expensive operation, but only called for the ~10-20 objects on each retention path.
    /// </summary>
    private static string ResolveFieldName(ClrHeap heap, ulong parentAddr, ulong childAddr)
    {
        try
        {
            var parentObj = heap.GetObject(parentAddr);
            if (!parentObj.IsValid || parentObj.Type is null)
                return "?";

            foreach (var reference in parentObj.EnumerateReferencesWithFields(
                carefully: true, considerDependantHandles: false))
            {
                if (reference.Object.Address == childAddr)
                {
                    return reference.IsArrayElement
                        ? "[*]"
                        : reference.Field?.Name ?? "?";
                }
            }
        }
        catch (Exception)
        {
            // Corrupted parent — return unknown
        }

        return "?";
    }

    /// <summary>Gets the type name of an object at the given address, or "?" if unknown.</summary>
    private static string GetTypeName(ClrHeap heap, ulong address)
    {
        var type = heap.GetObjectType(address);
        return type?.Name ?? "?";
    }

    private RootAnalysisResult BuildResult(
        IReadOnlyList<string> targetTypeNames,
        List<TypeRootAnalysis> typeAnalyses,
        List<string> skippedTypes,
        Stopwatch globalSw,
        bool wasTimedOut)
    {
        globalSw.Stop();

        var analyzed = new HashSet<string>(typeAnalyses.Select(t => t.TypeName));
        var skipped = new HashSet<string>(skippedTypes);
        foreach (var t in targetTypeNames)
        {
            if (!analyzed.Contains(t) && !skipped.Contains(t))
                skippedTypes.Add(t);
        }

        _logger.LogInformation(
            "Root analysis complete in {Duration}ms: {TypeCount} types analyzed, {SkipCount} skipped, timedOut={TimedOut}",
            globalSw.ElapsedMilliseconds,
            typeAnalyses.Count,
            skippedTypes.Count,
            wasTimedOut);

        return new RootAnalysisResult
        {
            AnalyzedAtUtc = DateTimeOffset.UtcNow,
            TypeAnalyses = typeAnalyses,
            TotalDuration = globalSw.Elapsed,
            WasTimedOut = wasTimedOut,
            SkippedTypes = skippedTypes
        };
    }

    /// <summary>
    /// Shortens a fully-qualified type name to its last segment for readability.
    /// e.g. "Microsoft.Extensions.DependencyInjection.ServiceCollection" → "ServiceCollection"
    ///      "System.Collections.Generic.List&lt;System.Byte[]&gt;" → "List&lt;Byte[]&gt;"
    /// </summary>
    private static string ShortenTypeName(string fullName)
    {
        var angleBracketDepth = 0;
        var lastDot = -1;
        for (var i = 0; i < fullName.Length; i++)
        {
            switch (fullName[i])
            {
                case '<': angleBracketDepth++; break;
                case '>': angleBracketDepth--; break;
                case '.' when angleBracketDepth == 0: lastDot = i; break;
            }
        }

        var shortOuter = lastDot >= 0 ? fullName[(lastDot + 1)..] : fullName;

        var openAngle = shortOuter.IndexOf('<');
        if (openAngle < 0)
            return shortOuter;

        var closeAngle = shortOuter.LastIndexOf('>');
        if (closeAngle <= openAngle)
            return shortOuter;

        var prefix = shortOuter[..openAngle];
        var innerArgs = shortOuter[(openAngle + 1)..closeAngle];

        var shortenedArgs = new List<string>();
        var argStart = 0;
        var depth = 0;
        for (var i = 0; i < innerArgs.Length; i++)
        {
            switch (innerArgs[i])
            {
                case '<': depth++; break;
                case '>': depth--; break;
                case ',' when depth == 0:
                    shortenedArgs.Add(ShortenTypeName(innerArgs[argStart..i].Trim()));
                    argStart = i + 1;
                    break;
            }
        }
        shortenedArgs.Add(ShortenTypeName(innerArgs[argStart..].Trim()));

        return $"{prefix}<{string.Join(", ", shortenedArgs)}>";
    }

    /// <summary>
    /// Maximum number of hops from target to user-code type for it to count as
    /// "direct ownership". A user-code type at depth 3 (e.g. Byte[] → Byte[][] →
    /// List&lt;Byte[]&gt; → MemoryLeakService) is a direct owner. A Blazor component
    /// at depth 8+ through framework plumbing is incidental — not actionable.
    /// </summary>
    private const int MaxDirectOwnershipDepth = 4;

    /// <summary>Internal accumulator for root hits before grouping.</summary>
    private readonly record struct RootHit(
        string RootKind,
        string RootObjectTypeName,
        ulong RootAddress,
        string? RetentionPath,
        int Count,
        bool HasUserCode,
        int UserCodeDepth);
}
