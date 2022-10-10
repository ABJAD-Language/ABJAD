using ABJAD.ParseEngine.Types;
using Xunit;

namespace ABJAD.ParseEngine.Test.Types;

public class CustomDataTypeTest
{
    [Fact]
    private void ReturnsCustomValue()
    {
        Assert.Equal("CustomValue", new CustomDataType("CustomValue").GetValue());
    }
}