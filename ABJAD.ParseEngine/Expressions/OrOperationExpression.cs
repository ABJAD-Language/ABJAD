namespace ABJAD.ParseEngine.Expressions;

public class OrOperationExpression : BinaryLogicalExpression
{
    public OrOperationExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand, secondOperand)
    {
    }
}