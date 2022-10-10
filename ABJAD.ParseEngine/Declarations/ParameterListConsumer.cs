using ABJAD.ParseEngine.Shared;
using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Declarations;

public class ParameterListConsumer : IParameterListConsumer
{
    private readonly ITokenConsumer tokenConsumer;
    private readonly ITypeConsumer typeConsumer;

    public ParameterListConsumer(ITokenConsumer tokenConsumer, ITypeConsumer typeConsumer)
    {
        Guard.Against.Null(tokenConsumer);
        Guard.Against.Null(typeConsumer);
        this.tokenConsumer = tokenConsumer;
        this.typeConsumer = typeConsumer;
    }

    public List<FunctionParameter> Consume()
    {
        var parameters = new List<FunctionParameter>();
        while (!tokenConsumer.CanConsume(TokenType.CLOSE_PAREN))
        {
            var paramType = typeConsumer.Consume();
            var paramName = tokenConsumer.Consume(TokenType.ID).Content;

            parameters.Add(new FunctionParameter(paramType.GetValue(), paramName));

            if (!tokenConsumer.CanConsume(TokenType.COMMA))
            {
                break;
            }

            tokenConsumer.Consume(TokenType.COMMA);
        }

        return parameters;
    }
}