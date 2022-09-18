namespace ABJAD.ParseEngine.Expressions;

public class MultiplicationExpression : BinaryExpression
{
    public MultiplicationExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand, secondOperand)
    {
    }
}