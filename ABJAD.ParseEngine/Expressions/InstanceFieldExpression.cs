using ABJAD.ParseEngine.Shared;

namespace ABJAD.ParseEngine.Expressions;

public class InstanceFieldExpression : Expression
{
    public InstanceFieldExpression(Token instance, Token field)
    {
        Instance = instance;
        Field = field;
    }

    public Token Instance { get; }
    public Token Field { get; }
}