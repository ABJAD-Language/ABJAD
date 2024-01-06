using ABJAD.Parser.Domain.Primitives;

namespace ABJAD.Test.Parser.Domain.Primitives;

public class IdentifierPrimitiveTest
{
    [Fact]
    private void StoresValueCorrectlyOnHappyPath()
    {
        var identifier = IdentifierPrimitive.From("ب");
        Assert.Equal("ب", identifier.Value);
    }

    [Fact]
    private void ThrowsExceptionIfValueIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => IdentifierPrimitive.From(null));
    }
}