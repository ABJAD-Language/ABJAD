using ABJAD.Parser.Domain.Primitives;

namespace ABJAD.Test.Parser.Domain.Primitives;

public class NullPrimitiveTest
{
    [Fact]
    private void ValueIsNull()
    {
        var nullPrimitive = NullPrimitive.Instance();
        Assert.Null(nullPrimitive.Value);
    }
}