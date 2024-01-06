namespace ABJAD.Parser.Domain.Expressions.Binary;

public class GreaterCheckExpression : BinaryLogicalExpression
{
    public GreaterCheckExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand, secondOperand)
    {
    }
}