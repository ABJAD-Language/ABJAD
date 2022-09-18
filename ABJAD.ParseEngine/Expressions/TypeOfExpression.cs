namespace ABJAD.ParseEngine.Expressions;

public class TypeOfExpression : Expression
{
    public TypeOfExpression(Expression target)
    {
        Target = target;
    }

    public Expression Target { get; }
}