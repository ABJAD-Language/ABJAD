using ABJAD.Parser.Domain.Types;

namespace ABJAD.Test.Parser.Domain.Types;

public class CustomDataTypeTest
{
    [Fact]
    private void ReturnsCustomValue()
    {
        Assert.Equal("CustomValue", new CustomDataType("CustomValue").GetValue());
    }
}