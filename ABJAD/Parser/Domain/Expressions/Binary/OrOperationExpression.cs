namespace ABJAD.Parser.Domain.Expressions.Binary;

public class OrOperationExpression : BinaryLogicalExpression
{
    public OrOperationExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand, secondOperand)
    {
    }
}