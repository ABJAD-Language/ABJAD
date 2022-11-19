namespace ABJAD.ParseEngine.Service.Expressions.Unary;

public class NegationExpressionApiModel : UnaryExpressionApiModel
{
    public NegationExpressionApiModel(ExpressionApiModel target) : base("expression.negation", target)
    {
    }
}