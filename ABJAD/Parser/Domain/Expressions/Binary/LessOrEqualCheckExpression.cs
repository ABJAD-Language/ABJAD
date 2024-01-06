namespace ABJAD.Parser.Domain.Expressions.Binary;

public class LessOrEqualCheckExpression : BinaryLogicalExpression
{
    public LessOrEqualCheckExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand,
        secondOperand)
    {
    }
}