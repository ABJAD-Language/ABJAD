namespace ABJAD.ParseEngine.Expressions.Binary;

public class OrOperationExpression : BinaryLogicalExpression
{
    public OrOperationExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand, secondOperand)
    {
    }
}