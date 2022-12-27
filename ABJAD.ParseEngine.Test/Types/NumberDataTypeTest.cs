using ABJAD.ParseEngine.Types;
using Xunit;

namespace ABJAD.ParseEngine.Test.Types;

public class NumberDataTypeTest
{
    [Fact]
    private void ReturnsNumberValue()
    {
        Assert.Equal("رقم", new NumberDataType().GetValue());
    }
}