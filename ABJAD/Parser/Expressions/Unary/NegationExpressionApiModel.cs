namespace ABJAD.Parser.Expressions.Unary;

public class NegationExpressionApiModel : UnaryExpressionApiModel
{
    public NegationExpressionApiModel(ExpressionApiModel target) : base("negation", target)
    {
    }
}