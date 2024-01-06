namespace ABJAD.Parser.Domain.Expressions;

public class CallExpression : Expression
{
    public CallExpression(PrimitiveExpression method, List<Expression> arguments)
    {
        Method = method;
        Arguments = arguments;
    }

    public PrimitiveExpression Method { get; }
    public List<Expression> Arguments { get; }
}