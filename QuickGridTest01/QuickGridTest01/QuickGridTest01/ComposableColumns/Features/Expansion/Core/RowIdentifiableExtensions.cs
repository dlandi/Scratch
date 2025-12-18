namespace QuickGridTest01.ComposableColumns.Features.Expansion.Core;

/// <summary>
/// Extension methods for <see cref="IRowIdentifiable"/>.
/// </summary>
public static class RowIdentifiableExtensions
{
    /// <summary>
    /// Returns true if this is a spacer row (Id &lt; 0).
    /// </summary>
    public static bool IsSpacer<T>(this T item) where T : IRowIdentifiable
        => item.Id < 0;
}
