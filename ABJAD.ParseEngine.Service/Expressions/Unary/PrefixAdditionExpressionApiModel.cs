namespace ABJAD.ParseEngine.Service.Expressions.Unary;

public class PrefixAdditionExpressionApiModel : UnaryExpressionApiModel
{
    public PrefixAdditionExpressionApiModel(ExpressionApiModel target) : base("prefix.addition", target)
    {
    }
}