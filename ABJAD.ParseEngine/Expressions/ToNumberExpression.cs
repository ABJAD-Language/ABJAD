namespace ABJAD.ParseEngine.Expressions;

public class ToNumberExpression : Expression
{
    public ToNumberExpression(Expression target)
    {
        Target = target;
    }

    public Expression Target { get; }
}