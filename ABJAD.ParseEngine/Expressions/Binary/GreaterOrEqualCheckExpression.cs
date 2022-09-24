namespace ABJAD.ParseEngine.Expressions.Binary;

public class GreaterOrEqualCheckExpression : BinaryLogicalExpression
{
    public GreaterOrEqualCheckExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand,
        secondOperand)
    {
    }
}