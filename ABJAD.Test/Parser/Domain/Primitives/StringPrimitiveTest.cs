using ABJAD.Parser.Domain.Primitives;

namespace ABJAD.Test.Parser.Domain.Primitives;

public class StringPrimitiveTest
{
    [Fact]
    private void StoresValueCorrectlyOnHappyPath()
    {
        var stringConstant = StringPrimitive.From("مرحبا بالعالم");
        Assert.Equal("مرحبا بالعالم", stringConstant.Value);
    }

    [Fact]
    private void ThrowsExceptionIfValueIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => StringPrimitive.From(null));
    }
}