namespace ABJAD.Parser.Expressions.Binary;

public class MultiplicationExpressionApiModel : BinaryExpressionApiModel
{
    public MultiplicationExpressionApiModel(ExpressionApiModel firstOperand, ExpressionApiModel secondOperand) : base("multiplication", firstOperand, secondOperand)
    {
    }
}