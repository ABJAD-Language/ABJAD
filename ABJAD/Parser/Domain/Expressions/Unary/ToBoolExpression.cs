namespace ABJAD.Parser.Domain.Expressions.Unary;

public class ToBoolExpression : Expression
{
    public ToBoolExpression(Expression target)
    {
        Target = target;
    }

    public Expression Target { get; }
}