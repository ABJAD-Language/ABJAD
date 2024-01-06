namespace ABJAD.Parser.Domain.Expressions.Binary;

public class MultiplicationExpression : BinaryExpression
{
    public MultiplicationExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand,
        secondOperand)
    {
    }
}