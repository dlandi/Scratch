namespace QuickGridTest01.ComposableColumns.Core;

/// <summary>
/// Minimal, Core-owned detection helpers for identifying grouping synthetic row ids.
///
/// Grouping synthetic ids are negative ints whose absolute value encodes:
/// - kind (bits 30..24)
/// - groupId (bits 23..8)
/// - offset (bits 7..0)
///
/// This API intentionally does not expose encode/decode helpers.
/// Feature-level code owns the encoder/decoder.
/// </summary>
public static class GroupingSyntheticRowId
{
    private const int KindShift = 24;
    private const int KindMask = 0x7F;

    // Kind values are part of the spec contract.
    private const int MarkerKind = 0x01;
    private const int SpacerKind = 0x02;

    /// <summary>
    /// Returns true if <paramref name="id"/> looks like a grouping synthetic id.
    /// </summary>
    public static bool IsGroupingSynthetic(int id)
    {
        if (id >= 0)
            return false;

        var payload = Math.Abs(id);
        var kind = (payload >> KindShift) & KindMask;
        return kind is MarkerKind or SpacerKind;
    }

    /// <summary>
    /// Returns true if <paramref name="id"/> is a grouping header marker row id.
    /// </summary>
    public static bool IsGroupHeaderMarker(int id)
    {
        if (id >= 0)
            return false;

        var payload = Math.Abs(id);
        var kind = (payload >> KindShift) & KindMask;
        return kind == MarkerKind;
    }

    /// <summary>
    /// Returns true if <paramref name="id"/> is a grouping header spacer row id.
    /// </summary>
    public static bool IsGroupHeaderSpacer(int id)
    {
        if (id >= 0)
            return false;

        var payload = Math.Abs(id);
        var kind = (payload >> KindShift) & KindMask;
        return kind == SpacerKind;
    }
}
