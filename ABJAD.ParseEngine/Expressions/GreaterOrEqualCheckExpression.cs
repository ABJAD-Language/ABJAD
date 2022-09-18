namespace ABJAD.ParseEngine.Expressions;

public class GreaterOrEqualCheckExpression : BinaryLogicalExpression
{
    public GreaterOrEqualCheckExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand, secondOperand)
    {
    }
}