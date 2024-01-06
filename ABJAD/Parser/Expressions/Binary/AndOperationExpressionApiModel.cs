namespace ABJAD.Parser.Expressions.Binary;

public class AndOperationExpressionApiModel : BinaryExpressionApiModel
{
    public AndOperationExpressionApiModel(ExpressionApiModel firstOperand, ExpressionApiModel secondOperand) : base("and", firstOperand, secondOperand)
    {
    }
}