namespace ABJAD.ParseEngine.Expressions;

public class ToBoolExpression : Expression
{
    public ToBoolExpression(Expression target)
    {
        Target = target;
    }

    public Expression Target { get; }
}