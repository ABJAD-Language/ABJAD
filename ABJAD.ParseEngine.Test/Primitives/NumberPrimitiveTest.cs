using System;
using ABJAD.ParseEngine.Primitives;
using Xunit;

namespace ABJAD.ParseEngine.Test.Primitives;

public class NumberPrimitiveTest
{
    [Fact]
    private void StoresValueAsDoubleCorrectly()
    {
        var numberConstant = NumberPrimitive.From("1.2");
        Assert.Equal(1.2, numberConstant.Value);
    }

    [Fact]
    private void StoresIntegerAsDoubleCorrectly()
    {
        var numberConstant = NumberPrimitive.From("3");
        Assert.Equal(3.0, numberConstant.Value);
    }

    [Fact]
    private void ThrowsExceptionIfStringValueHasInvalidFormat()
    {
        Assert.Throws<FormatException>(() => NumberPrimitive.From("1.3.2"));
    }

    [Fact]
    private void CastsStringValueWithNoOnesDigitCorrectly()
    {
        var numberConstant = NumberPrimitive.From(".23");
        Assert.Equal(0.23, numberConstant.Value);
    }

    [Fact]
    private void ThrowsExceptionIfValueIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => NumberPrimitive.From(null));
    }
}