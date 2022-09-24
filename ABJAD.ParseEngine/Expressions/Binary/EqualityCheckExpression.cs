namespace ABJAD.ParseEngine.Expressions.Binary;

public class EqualityCheckExpression : BinaryLogicalExpression
{
    public EqualityCheckExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand,
        secondOperand)
    {
    }
}