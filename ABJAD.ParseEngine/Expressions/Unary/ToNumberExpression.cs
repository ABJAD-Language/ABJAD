namespace ABJAD.ParseEngine.Expressions.Unary;

public class ToNumberExpression : Expression
{
    public ToNumberExpression(Expression target)
    {
        Target = target;
    }

    public Expression Target { get; }
}