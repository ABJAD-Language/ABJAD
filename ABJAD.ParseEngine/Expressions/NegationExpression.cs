namespace ABJAD.ParseEngine.Expressions;

public class NegationExpression : UnaryLogicalExpression
{
    public NegationExpression(Expression target) : base(target)
    {
    }
}