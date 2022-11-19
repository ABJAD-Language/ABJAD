namespace ABJAD.ParseEngine.Service.Expressions.Unary;

public class ToBoolExpressionApiModel : UnaryExpressionApiModel
{
    public ToBoolExpressionApiModel(ExpressionApiModel target) : base("expression.toBool", target)
    {
    }
}