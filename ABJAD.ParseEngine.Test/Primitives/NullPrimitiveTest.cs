using ABJAD.ParseEngine.Primitives;
using Xunit;

namespace ABJAD.ParseEngine.Test.Primitives;

public class NullPrimitiveTest
{
    [Fact]
    private void ValueIsNull()
    {
        var nullPrimitive = new NullPrimitive();
        Assert.Null(nullPrimitive.Value);
    }
}