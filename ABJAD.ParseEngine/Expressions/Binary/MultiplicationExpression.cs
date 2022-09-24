namespace ABJAD.ParseEngine.Expressions.Binary;

public class MultiplicationExpression : BinaryExpression
{
    public MultiplicationExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand,
        secondOperand)
    {
    }
}