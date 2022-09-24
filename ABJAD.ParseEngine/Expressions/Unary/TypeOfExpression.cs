namespace ABJAD.ParseEngine.Expressions.Unary;

public class TypeOfExpression : Expression
{
    public TypeOfExpression(Expression target)
    {
        Target = target;
    }

    public Expression Target { get; }
}