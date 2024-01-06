using ABJAD.Parser.Domain.Types;

namespace ABJAD.Test.Parser.Domain.Types;

public class VoidDataTypeTest
{
    [Fact]
    private void ReturnsVoidValue()
    {
        Assert.Equal("VOID", new VoidDataType().GetValue());
    }
}