namespace ABJAD.Parser.Expressions.Binary;

public class LessOrEqualCheckExpressionApiModel : BinaryExpressionApiModel
{
    public LessOrEqualCheckExpressionApiModel(ExpressionApiModel firstOperand, ExpressionApiModel secondOperand) : base("lessOrEqualCheck", firstOperand, secondOperand)
    {
    }
}