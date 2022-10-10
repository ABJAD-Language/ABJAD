using ABJAD.ParseEngine.Types;
using Xunit;

namespace ABJAD.ParseEngine.Test.Types;

public class NumberDataTypeTest
{
    [Fact]
    private void ReturnsNumberValue()
    {
        Assert.Equal("NUMBER", new NumberDataType().GetValue());
    }
}