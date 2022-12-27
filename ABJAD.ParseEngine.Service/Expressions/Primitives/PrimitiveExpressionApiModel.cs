using ABJAD.ParseEngine.Service.Expressions;

namespace ABJAD.ParseEngine.Service.Primitives;

public abstract class PrimitiveExpressionApiModel : ExpressionApiModel
{
    public PrimitiveExpressionApiModel(string type)
    {
        Type = $"expression.primitive.{type}";
    }
}