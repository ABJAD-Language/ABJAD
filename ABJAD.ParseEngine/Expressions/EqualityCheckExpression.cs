namespace ABJAD.ParseEngine.Expressions;

public class EqualityCheckExpression : BinaryLogicalExpression
{
    public EqualityCheckExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand, secondOperand)
    {
    }
}