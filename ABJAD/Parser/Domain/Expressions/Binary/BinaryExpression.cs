namespace ABJAD.Parser.Domain.Expressions.Binary;

public abstract class BinaryExpression : Expression
{
    protected BinaryExpression(Expression firstOperand, Expression secondOperand)
    {
        FirstOperand = firstOperand;
        SecondOperand = secondOperand;
    }

    public Expression FirstOperand { get; }
    public Expression SecondOperand { get; }
}