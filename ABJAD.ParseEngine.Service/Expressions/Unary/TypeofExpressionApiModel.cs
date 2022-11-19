namespace ABJAD.ParseEngine.Service.Expressions.Unary;

public class TypeofExpressionApiModel : UnaryExpressionApiModel
{
    public TypeofExpressionApiModel(ExpressionApiModel target) : base("expression.typeof", target)
    {
    }
}