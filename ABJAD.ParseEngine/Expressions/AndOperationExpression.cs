namespace ABJAD.ParseEngine.Expressions;

public class AndOperationExpression : BinaryLogicalExpression
{
    public AndOperationExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand, secondOperand)
    {
    }
}