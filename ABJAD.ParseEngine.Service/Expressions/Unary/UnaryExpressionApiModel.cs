namespace ABJAD.ParseEngine.Service.Expressions.Unary;

public abstract class UnaryExpressionApiModel : ExpressionApiModel
{
    public ExpressionApiModel Target { get; }

    protected UnaryExpressionApiModel(string type, ExpressionApiModel target)
    {
        Target = target;
        Type = type;
    }
}