namespace ABJAD.ParseEngine.Service.Expressions.Binary;

public class AdditionExpressionApiModel : BinaryExpressionApiModel
{
    public AdditionExpressionApiModel(ExpressionApiModel firstOperand, ExpressionApiModel secondOperand) : base("addition", firstOperand, secondOperand)
    {
    }
}