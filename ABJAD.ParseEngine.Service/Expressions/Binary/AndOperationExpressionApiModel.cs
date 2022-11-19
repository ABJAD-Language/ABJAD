namespace ABJAD.ParseEngine.Service.Expressions.Binary;

public class AndOperationExpressionApiModel : BinaryExpressionApiModel
{
    public AndOperationExpressionApiModel(ExpressionApiModel firstOperand, ExpressionApiModel secondOperand) : base("expression.and", firstOperand, secondOperand)
    {
    }
}