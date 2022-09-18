namespace ABJAD.ParseEngine.Expressions;

public class LessOrEqualCheckExpression : BinaryLogicalExpression
{
    public LessOrEqualCheckExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand, secondOperand)
    {
    }
}