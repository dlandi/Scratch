namespace AppSysMetrics.Diagnostics;

/// <summary>
/// Shared type classification helpers for leak suspect filtering.
/// Stateless — safe to call from any context.
/// </summary>
public static class TypeClassification
{
    /// <summary>
    /// Returns true if a dot-free type name is a well-known primitive, array, or collection
    /// type that could legitimately be a leak suspect (e.g. "Byte[]", "String", "Object[]").
    /// These are the types that root analysis can trace back to user code via retention paths.
    /// </summary>
    public static bool IsWellKnownContainerType(string typeName)
    {
        var baseName = typeName;
        while (baseName.EndsWith("[]", StringComparison.Ordinal))
            baseName = baseName[..^2];

        return baseName is "Byte" or "String" or "Char" or "Int32" or "Int64"
            or "UInt32" or "UInt64" or "Int16" or "UInt16" or "Double" or "Single"
            or "Boolean" or "Object" or "IntPtr" or "UIntPtr" or "Decimal"
            or "SByte" or "Guid" or "DateTime" or "DateTimeOffset" or "TimeSpan";
    }

    /// <summary>
    /// Returns true if a type belongs to a Microsoft.* namespace that represents
    /// developer-controlled infrastructure. Growth in these types is actionable —
    /// e.g. CacheEntry accumulation means an unbounded IMemoryCache, EntityEntry
    /// growth means a long-lived DbContext with tracking enabled.
    /// </summary>
    public static bool IsDeveloperFacingFrameworkType(string typeName)
    {
        return typeName.StartsWith("Microsoft.Extensions.Caching.", StringComparison.Ordinal)
            || typeName.StartsWith("Microsoft.EntityFrameworkCore.", StringComparison.Ordinal)
            || typeName.StartsWith("Microsoft.AspNetCore.SignalR.", StringComparison.Ordinal);
    }

    /// <summary>
    /// Returns true if a type is definitely framework-only — not user code and not a
    /// System.* container type that root analysis can trace. The <paramref name="isUserCode"/>
    /// delegate provides assembly-level classification (typically <c>GcRootAnalyzer.IsUserCode</c>).
    ///
    /// Pipeline:
    ///   1. User-code types pass through (application types)
    ///   2. System.* container types pass through (legitimate suspects)
    ///   3. Dot-free types only pass if they match the well-known primitive allowlist
    ///   4. Developer-facing framework types pass through (actionable growth)
    ///   5. Everything else is framework-only (filtered out)
    /// </summary>
    public static bool IsFrameworkOnlyType(string typeName, Func<string, bool> isUserCode)
    {
        if (isUserCode(typeName))
            return false;

        if (typeName.StartsWith("System.", StringComparison.Ordinal))
            return false;

        if (!typeName.Contains('.'))
            return !IsWellKnownContainerType(typeName);

        if (IsDeveloperFacingFrameworkType(typeName))
            return false;

        return true;
    }
}
