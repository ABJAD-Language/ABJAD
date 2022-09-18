using System;
using ABJAD.ParseEngine.Primitives;
using Xunit;

namespace ABJAD.ParseEngine.Test.Primitives;

public class StringPrimitiveTest
{
    [Fact]
    private void StoresValueCorrectlyOnHappyPath()
    {
        var stringConstant = new StringPrimitive("مرحبا بالعالم");
        Assert.Equal("مرحبا بالعالم", stringConstant.Value);
    }

    [Fact]
    private void ThrowsExceptionIfValueIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new StringPrimitive(null));
    }
}