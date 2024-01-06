namespace ABJAD.Parser.Domain.Expressions.Binary;

public class LessCheckExpression : BinaryLogicalExpression
{
    public LessCheckExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand, secondOperand)
    {
    }
}