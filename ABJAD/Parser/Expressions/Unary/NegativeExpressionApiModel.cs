namespace ABJAD.Parser.Expressions.Unary;

public class NegativeExpressionApiModel : UnaryExpressionApiModel
{
    public NegativeExpressionApiModel(ExpressionApiModel target) : base("negative", target)
    {
    }
}