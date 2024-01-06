namespace ABJAD.Parser.Domain.Expressions.Binary;

public class BinaryLogicalExpression : BinaryExpression
{
    public BinaryLogicalExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand,
        secondOperand)
    {
    }
}