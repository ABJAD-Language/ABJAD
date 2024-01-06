using ABJAD.Parser.Domain.Primitives;

namespace ABJAD.Test.Parser.Domain.Primitives;

public class BoolPrimitiveTest
{
    [Fact]
    private void TrueReturnsBoolConstantWithValueTrue()
    {
        var boolConstant = BoolPrimitive.True();
        Assert.True(boolConstant.Value);
    }

    [Fact]
    private void FalseReturnsBoolConstantWithValueFalse()
    {
        var boolConstant = BoolPrimitive.False();
        Assert.False(boolConstant.Value);
    }
}