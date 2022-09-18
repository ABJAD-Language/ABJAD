using ABJAD.ParseEngine.Shared;

namespace ABJAD.ParseEngine.Expressions;

public class InstanceMethodCallExpression : CallExpression
{
    public InstanceMethodCallExpression(Token @class, Token method, List<Expression> arguments) : base(method, arguments)
    {
        Class = @class;
    }

    public Token Class { get; }
}