namespace QuickGridTest01.ComposableColumns.Features.Grouping;

public static class GroupHeaderRowId
{
    private const int KindShift = 24;
    private const int KindMask = 0x7F;

    private const int MarkerKind = 0x01;
    private const int SpacerKind = 0x02;

    public static int EncodeGroupHeaderId(int groupId)
    {
        if (groupId is < 1 or > 0xFFFF)
            throw new ArgumentOutOfRangeException(nameof(groupId), "groupId must be in the range 1..65535.");

        var payload = (MarkerKind << KindShift) | (groupId << 8);
        return -payload;
    }

    public static int EncodeGroupHeaderSpacerId(int groupId, int offset)
    {
        if (groupId is < 1 or > 0xFFFF)
            throw new ArgumentOutOfRangeException(nameof(groupId), "groupId must be in the range 1..65535.");

        if (offset is < 1 or > 0xFF)
            throw new ArgumentOutOfRangeException(nameof(offset), "offset must be in the range 1..255.");

        var payload = (SpacerKind << KindShift) | (groupId << 8) | offset;
        return -payload;
    }

    public static int GetGroupId(int syntheticId)
    {
        if (syntheticId >= 0)
            throw new ArgumentException("The supplied id is not a grouping synthetic id.", nameof(syntheticId));

        var payload = Math.Abs(syntheticId);
        return (payload >> 8) & 0xFFFF;
    }

    public static int GetSpacerOffset(int syntheticId)
    {
        if (syntheticId >= 0)
            throw new ArgumentException("The supplied id is not a grouping synthetic id.", nameof(syntheticId));

        var payload = Math.Abs(syntheticId);
        var kind = (payload >> KindShift) & KindMask;

        if (kind != SpacerKind)
            throw new ArgumentException("The supplied id is not a grouping spacer id.", nameof(syntheticId));

        return payload & 0xFF;
    }
}
