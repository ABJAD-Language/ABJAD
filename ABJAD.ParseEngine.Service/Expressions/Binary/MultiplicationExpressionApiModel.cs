namespace ABJAD.ParseEngine.Service.Expressions.Binary;

public class MultiplicationExpressionApiModel : BinaryExpressionApiModel
{
    public MultiplicationExpressionApiModel(ExpressionApiModel firstOperand, ExpressionApiModel secondOperand) : base("multiplication", firstOperand, secondOperand)
    {
    }
}