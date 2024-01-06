namespace ABJAD.Parser.Domain.Expressions.Binary;

public class InequalityCheckExpression : BinaryLogicalExpression
{
    public InequalityCheckExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand,
        secondOperand)
    {
    }
}