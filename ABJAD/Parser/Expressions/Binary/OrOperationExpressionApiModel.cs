namespace ABJAD.Parser.Expressions.Binary;

public class OrOperationExpressionApiModel : BinaryExpressionApiModel
{
    public OrOperationExpressionApiModel(ExpressionApiModel firstOperand, ExpressionApiModel secondOperand) : base("or", firstOperand, secondOperand)
    {
    }
}