using ABJAD.ParseEngine.Types;
using Xunit;

namespace ABJAD.ParseEngine.Test.Types;

public class VoidDataTypeTest
{
    [Fact]
    private void ReturnsVoidValue()
    {
        Assert.Equal("VOID", new VoidDataType().GetValue()); 
    }
}