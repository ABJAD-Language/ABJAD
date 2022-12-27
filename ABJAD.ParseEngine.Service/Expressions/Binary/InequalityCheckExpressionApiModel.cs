namespace ABJAD.ParseEngine.Service.Expressions.Binary;

public class InequalityCheckExpressionApiModel : BinaryExpressionApiModel
{
    public InequalityCheckExpressionApiModel(ExpressionApiModel firstOperand, ExpressionApiModel secondOperand) : base("inequalityCheck", firstOperand, secondOperand)
    {
    }
}