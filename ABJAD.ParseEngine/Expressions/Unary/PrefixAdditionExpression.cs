namespace ABJAD.ParseEngine.Expressions.Unary;

public class PrefixAdditionExpression : UnaryExpression
{
    public PrefixAdditionExpression(Expression target) : base(target) // TODO make target IdentifierPrimitive
    {
    }
}