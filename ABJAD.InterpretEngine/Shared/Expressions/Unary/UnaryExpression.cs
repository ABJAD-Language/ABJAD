namespace ABJAD.InterpretEngine.Shared.Expressions.Unary;

public abstract class UnaryExpression : Expression
{
    public Expression Target { get; set; }
}