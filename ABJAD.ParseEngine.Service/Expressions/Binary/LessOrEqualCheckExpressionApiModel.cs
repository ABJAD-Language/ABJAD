namespace ABJAD.ParseEngine.Service.Expressions.Binary;

public class LessOrEqualCheckExpressionApiModel : BinaryExpressionApiModel
{
    public LessOrEqualCheckExpressionApiModel(ExpressionApiModel firstOperand, ExpressionApiModel secondOperand) : base("lessOrEqualCheck", firstOperand, secondOperand)
    {
    }
}