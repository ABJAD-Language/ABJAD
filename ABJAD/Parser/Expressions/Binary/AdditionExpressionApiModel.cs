namespace ABJAD.Parser.Expressions.Binary;

public class AdditionExpressionApiModel : BinaryExpressionApiModel
{
    public AdditionExpressionApiModel(ExpressionApiModel firstOperand, ExpressionApiModel secondOperand) : base("addition", firstOperand, secondOperand)
    {
    }
}