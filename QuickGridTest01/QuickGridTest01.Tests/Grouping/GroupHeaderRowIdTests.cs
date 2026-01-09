using QuickGridTest01.ComposableColumns.Core;
using QuickGridTest01.ComposableColumns.Features.Grouping;
using Xunit;

namespace QuickGridTest01.Tests.Grouping;

public class GroupHeaderRowIdTests
{
    [Fact]
    public void EncodeGroupHeaderId_WhenGroupIdOutOfRange_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => GroupHeaderRowId.EncodeGroupHeaderId(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => GroupHeaderRowId.EncodeGroupHeaderId(65536));
    }

    [Fact]
    public void EncodeGroupHeaderSpacerId_WhenInputsOutOfRange_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => GroupHeaderRowId.EncodeGroupHeaderSpacerId(0, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => GroupHeaderRowId.EncodeGroupHeaderSpacerId(1, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => GroupHeaderRowId.EncodeGroupHeaderSpacerId(1, 256));
    }

    [Fact]
    public void EncodeMarkerAndSpacer_AreNegativeAndDetectable()
    {
        var marker = GroupHeaderRowId.EncodeGroupHeaderId(1);
        var spacer = GroupHeaderRowId.EncodeGroupHeaderSpacerId(1, 1);

        Assert.True(marker < 0);
        Assert.True(spacer < 0);

        Assert.True(GroupingSyntheticRowId.IsGroupingSynthetic(marker));
        Assert.True(GroupingSyntheticRowId.IsGroupingSynthetic(spacer));

        Assert.True(GroupingSyntheticRowId.IsGroupHeaderMarker(marker));
        Assert.False(GroupingSyntheticRowId.IsGroupHeaderSpacer(marker));

        Assert.False(GroupingSyntheticRowId.IsGroupHeaderMarker(spacer));
        Assert.True(GroupingSyntheticRowId.IsGroupHeaderSpacer(spacer));
    }

    [Fact]
    public void Decode_WhenNotSynthetic_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => GroupHeaderRowId.GetGroupId(1));
        Assert.Throws<ArgumentException>(() => GroupHeaderRowId.GetSpacerOffset(1));
    }

    [Fact]
    public void GetGroupId_ReturnsEncodedGroupId_ForMarkerAndSpacer()
    {
        var marker = GroupHeaderRowId.EncodeGroupHeaderId(42);
        var spacer = GroupHeaderRowId.EncodeGroupHeaderSpacerId(42, 7);

        Assert.Equal(42, GroupHeaderRowId.GetGroupId(marker));
        Assert.Equal(42, GroupHeaderRowId.GetGroupId(spacer));
    }

    [Fact]
    public void GetSpacerOffset_WhenNotSpacer_ThrowsArgumentException()
    {
        var marker = GroupHeaderRowId.EncodeGroupHeaderId(42);
        Assert.Throws<ArgumentException>(() => GroupHeaderRowId.GetSpacerOffset(marker));
    }

    [Fact]
    public void GetSpacerOffset_ReturnsEncodedOffset_ForSpacer()
    {
        var spacer = GroupHeaderRowId.EncodeGroupHeaderSpacerId(42, 7);
        Assert.Equal(7, GroupHeaderRowId.GetSpacerOffset(spacer));
    }
}
