using ABJAD.ParseEngine.Shared;

namespace ABJAD.ParseEngine.Expressions;

public class InstantiationExpression : Expression
{
    public InstantiationExpression(Token @class, List<Expression> arguments)
    {
        Class = @class;
        Arguments = arguments;
    }

    public Token Class { get; }
    public List<Expression> Arguments { get; }
}