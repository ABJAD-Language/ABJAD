using ABJAD.Parser.Domain.Types;

namespace ABJAD.Test.Parser.Domain.Types;

public class NumberDataTypeTest
{
    [Fact]
    private void ReturnsNumberValue()
    {
        Assert.Equal("رقم", new NumberDataType().GetValue());
    }
}