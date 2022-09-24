namespace ABJAD.ParseEngine.Expressions.Binary;

public class InequalityCheckExpression : BinaryLogicalExpression
{
    public InequalityCheckExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand,
        secondOperand)
    {
    }
}