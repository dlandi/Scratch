namespace ResourceScheduler.Components.Services;

/// <summary>
/// Which concrete <see cref="IClientService"/> implementation backs the
/// app at runtime. Selected by the user via the header dropdown when
/// <c>Features:BackendSwitcher:Enabled</c> is true.
/// </summary>
public enum BackendMode
{
    /// <summary>The Phase 1 in-process simulator with seeded fixture data.</summary>
    InMemory,

    /// <summary>The Phase 2 HTTP client against the Rust API.</summary>
    Rust,
}

/// <summary>One entry in the header backend dropdown.</summary>
public sealed record BackendOption(string Id, string Label);
