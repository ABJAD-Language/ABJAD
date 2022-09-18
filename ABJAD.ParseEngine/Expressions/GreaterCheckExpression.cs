namespace ABJAD.ParseEngine.Expressions;

public class GreaterCheckExpression : BinaryLogicalExpression
{
    public GreaterCheckExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand, secondOperand)
    {
    }
}