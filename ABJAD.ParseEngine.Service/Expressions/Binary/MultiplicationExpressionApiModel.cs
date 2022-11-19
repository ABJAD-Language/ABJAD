namespace ABJAD.ParseEngine.Service.Expressions.Binary;

public class MultiplicationExpressionApiModel : BinaryExpressionApiModel
{
    public MultiplicationExpressionApiModel(ExpressionApiModel firstOperand, ExpressionApiModel secondOperand) : base("expression.multiplication", firstOperand, secondOperand)
    {
    }
}