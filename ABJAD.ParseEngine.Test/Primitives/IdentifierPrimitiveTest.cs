using System;
using ABJAD.ParseEngine.Primitives;
using Xunit;

namespace ABJAD.ParseEngine.Test.Primitives;

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