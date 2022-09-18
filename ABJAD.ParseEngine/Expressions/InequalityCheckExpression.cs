namespace ABJAD.ParseEngine.Expressions;

public class InequalityCheckExpression : BinaryLogicalExpression
{
    public InequalityCheckExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand, secondOperand)
    {
    }
}