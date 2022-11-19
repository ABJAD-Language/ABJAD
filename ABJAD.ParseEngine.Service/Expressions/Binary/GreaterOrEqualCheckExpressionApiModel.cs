namespace ABJAD.ParseEngine.Service.Expressions.Binary;

public class GreaterOrEqualCheckExpressionApiModel : BinaryExpressionApiModel
{
    public GreaterOrEqualCheckExpressionApiModel(ExpressionApiModel firstOperand, ExpressionApiModel secondOperand) : base("expression.greaterOrEqualCheck", firstOperand, secondOperand)
    {
    }
}