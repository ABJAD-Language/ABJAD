namespace ABJAD.Parser.Expressions.Binary;

public class GreaterCheckExpressionApiModel : BinaryExpressionApiModel
{
    public GreaterCheckExpressionApiModel(ExpressionApiModel firstOperand, ExpressionApiModel secondOperand) : base("greaterCheck", firstOperand, secondOperand)
    {
    }
}