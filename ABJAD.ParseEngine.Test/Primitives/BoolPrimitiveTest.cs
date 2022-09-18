using ABJAD.ParseEngine.Primitives;
using Xunit;

namespace ABJAD.ParseEngine.Test.Primitives;

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