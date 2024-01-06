using ABJAD.Parser.Domain.Types;

namespace ABJAD.Test.Parser.Domain.Types;

public class BoolDataTypeTest
{
    [Fact]
    private void ReturnsBoolValue()
    {
        Assert.Equal("منطق", new BoolDataType().GetValue());
    }
}
