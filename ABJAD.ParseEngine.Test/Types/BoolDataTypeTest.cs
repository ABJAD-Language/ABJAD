using ABJAD.ParseEngine.Types;
using Xunit;

namespace ABJAD.ParseEngine.Test.Types;

public class BoolDataTypeTest
{
    [Fact]
    private void ReturnsBoolValue()
    {
        Assert.Equal("BOOL", new BoolDataType().GetValue());
    }
}
