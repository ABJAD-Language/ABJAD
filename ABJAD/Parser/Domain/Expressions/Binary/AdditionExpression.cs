namespace ABJAD.Parser.Domain.Expressions.Binary;

public class AdditionExpression : BinaryExpression
{
    public AdditionExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand, secondOperand)
    {
    }
}