namespace ABJAD.Interpreter.Domain.Shared.Expressions.Unary;

public abstract class UnaryExpression : Expression
{
    public Expression Target { get; set; }
}