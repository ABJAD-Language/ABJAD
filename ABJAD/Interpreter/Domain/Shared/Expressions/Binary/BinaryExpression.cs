namespace ABJAD.Interpreter.Domain.Shared.Expressions.Binary;

public abstract class BinaryExpression : Expression
{
    public Expression FirstOperand { get; set; }
    public Expression SecondOperand { get; set; }
}