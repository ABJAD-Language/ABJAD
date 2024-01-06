using ABJAD.Parser.Domain.Types;

namespace ABJAD.Test.Parser.Domain.Types;

public class StringDataTypeTest
{
    [Fact]
    private void ReturnsStringValue()
    {
        Assert.Equal("مقطع", new StringDataType().GetValue());
    }
}