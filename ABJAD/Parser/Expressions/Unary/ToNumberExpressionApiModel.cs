namespace ABJAD.Parser.Expressions.Unary;

public class ToNumberExpressionApiModel : UnaryExpressionApiModel
{
    public ToNumberExpressionApiModel(ExpressionApiModel target) : base("toNumber", target)
    {
    }
}