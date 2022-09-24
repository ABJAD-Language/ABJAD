namespace ABJAD.ParseEngine.Expressions.Binary;

public class LessOrEqualCheckExpression : BinaryLogicalExpression
{
    public LessOrEqualCheckExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand,
        secondOperand)
    {
    }
}