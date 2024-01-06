using ABJAD.Parser.Domain.Primitives;

namespace ABJAD.Parser.Domain.Expressions;

public class PrimitiveExpression : Expression
{
    public PrimitiveExpression(Primitive primitive)
    {
        Primitive = primitive;
    }

    public Primitive Primitive { get; }
}