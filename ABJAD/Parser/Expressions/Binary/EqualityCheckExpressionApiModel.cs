namespace ABJAD.Parser.Expressions.Binary;

public class EqualityCheckExpressionApiModel : BinaryExpressionApiModel
{
    public EqualityCheckExpressionApiModel(ExpressionApiModel firstOperand, ExpressionApiModel secondOperand) : base("equalityCheck", firstOperand, secondOperand)
    {
    }
}