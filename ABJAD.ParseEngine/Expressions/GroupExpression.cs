namespace ABJAD.ParseEngine.Expressions;

public class GroupExpression : Expression
{
    public GroupExpression(Expression target)
    {
        Target = target;
    }

    public Expression Target { get; }
}