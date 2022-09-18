using ABJAD.ParseEngine.Shared;

namespace ABJAD.ParseEngine.Expressions;

public class CallExpression : Expression
{
    public CallExpression(Token method, List<Expression> arguments)
    {
        Method = method;
        Arguments = arguments;
    }

    public Token Method { get; }
    public List<Expression> Arguments { get; }   
}