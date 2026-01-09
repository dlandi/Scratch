using QuickGridTest01.ComposableColumns.Features.Expansion.Data;
using Xunit;

namespace QuickGridTest01.Tests.Expansion;

public class SpacerRowFactoryTests
{
    [Fact]
    public void EncodeSpacerId_ProducesNegativeId()
    {
        var id = SpacerRowFactory.EncodeSpacerId(parentRowId: 123, offset: 1);
        Assert.True(id < 0);
    }

    [Fact]
    public void EncodeDecode_RoundTripsParentRowId_AndOffset()
    {
        var id = SpacerRowFactory.EncodeSpacerId(parentRowId: 42, offset: 7);

        Assert.Equal(42, SpacerRowFactory.GetParentRowId(id));
        Assert.Equal(7, SpacerRowFactory.GetSpacerOffset(id));
    }

    [Theory]
    [InlineData(-1, true)]
    [InlineData(-999, true)]
    [InlineData(0, false)]
    [InlineData(1, false)]
    public void IsSpacer_DetectsNegative(int id, bool expected)
    {
        Assert.Equal(expected, SpacerRowFactory.IsSpacer(id));
    }
}
