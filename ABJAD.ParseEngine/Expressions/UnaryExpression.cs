namespace ABJAD.ParseEngine.Expressions;

public abstract class UnaryExpression : Expression
{
    protected UnaryExpression(Expression target)
    {
        Target = target;
    }

    public Expression Target { get; }
}