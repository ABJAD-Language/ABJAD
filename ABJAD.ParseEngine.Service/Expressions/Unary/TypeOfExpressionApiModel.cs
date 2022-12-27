namespace ABJAD.ParseEngine.Service.Expressions.Unary;

public class TypeOfExpressionApiModel : UnaryExpressionApiModel
{
    public TypeOfExpressionApiModel(ExpressionApiModel target) : base("typeOf", target)
    {
    }
}