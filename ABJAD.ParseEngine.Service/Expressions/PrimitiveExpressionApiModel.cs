using ABJAD.ParseEngine.Service.Primitives;

namespace ABJAD.ParseEngine.Service.Expressions;

public class PrimitiveExpressionApiModel : ExpressionApiModel
{
    public PrimitiveApiModel Primitive { get; }

    public PrimitiveExpressionApiModel(PrimitiveApiModel primitive)
    {
        Primitive = primitive;
        Type = "expression.primitive";
    }
}