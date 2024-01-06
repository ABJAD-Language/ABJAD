namespace ABJAD.Parser.Domain.Expressions.Binary;

public class GreaterOrEqualCheckExpression : BinaryLogicalExpression
{
    public GreaterOrEqualCheckExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand,
        secondOperand)
    {
    }
}