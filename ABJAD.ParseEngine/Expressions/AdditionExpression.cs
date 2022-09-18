namespace ABJAD.ParseEngine.Expressions;

public class AdditionExpression : BinaryExpression
{
    public AdditionExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand, secondOperand)
    {
    }
}