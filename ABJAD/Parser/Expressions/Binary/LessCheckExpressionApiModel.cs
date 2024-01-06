namespace ABJAD.Parser.Expressions.Binary;

public class LessCheckExpressionApiModel : BinaryExpressionApiModel
{
    public LessCheckExpressionApiModel(ExpressionApiModel firstOperand, ExpressionApiModel secondOperand) : base("lessCheck", firstOperand, secondOperand)
    {
    }
}