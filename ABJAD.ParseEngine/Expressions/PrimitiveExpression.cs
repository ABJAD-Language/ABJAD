using ABJAD.ParseEngine.Primitives;

namespace ABJAD.ParseEngine.Expressions;

public class PrimitiveExpression : Expression
{
    public PrimitiveExpression(Primitive primitive)
    {
        Primitive = primitive;
    }

    public Primitive Primitive { get; }
}