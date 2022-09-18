namespace ABJAD.ParseEngine.Expressions;

public class LessCheckExpression : BinaryLogicalExpression
{
    public LessCheckExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand, secondOperand)
    {
    }
}