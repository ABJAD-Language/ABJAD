namespace ABJAD.Parser.Expressions.Primitives;

public abstract class PrimitiveExpressionApiModel : ExpressionApiModel
{
    public PrimitiveExpressionApiModel(string type)
    {
        Type = $"expression.primitive.{type}";
    }
}