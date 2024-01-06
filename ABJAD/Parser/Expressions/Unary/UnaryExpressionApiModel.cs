namespace ABJAD.Parser.Expressions.Unary;

public abstract class UnaryExpressionApiModel : ExpressionApiModel
{
    public ExpressionApiModel Target { get; }

    protected UnaryExpressionApiModel(string type, ExpressionApiModel target)
    {
        Target = target;
        Type = $"expression.unary.{type}";
    }
}