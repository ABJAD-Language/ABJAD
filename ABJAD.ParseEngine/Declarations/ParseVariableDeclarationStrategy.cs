using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Shared;
using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Declarations;

public class ParseVariableDeclarationStrategy : ParseDeclarationStrategy
{
    private readonly ITokenConsumer tokenConsumer;
    private readonly ExpressionParser expressionParser;
    private readonly ITypeConsumer typeConsumer;

    public ParseVariableDeclarationStrategy(ITokenConsumer tokenConsumer, ExpressionParser expressionParser,
        ITypeConsumer typeConsumer)
    {
        Guard.Against.Null(tokenConsumer);
        Guard.Against.Null(expressionParser);
        Guard.Against.Null(typeConsumer);
        this.tokenConsumer = tokenConsumer;
        this.expressionParser = expressionParser;
        this.typeConsumer = typeConsumer;
    }

    public Declaration Parse()
    {
        tokenConsumer.Consume(TokenType.VAR);
        var type = typeConsumer.Consume();
        var name = tokenConsumer.Consume(TokenType.ID);

        Expression? value = null;
        if (tokenConsumer.CanConsume(TokenType.EQUAL))
        {
            tokenConsumer.Consume(TokenType.EQUAL);
            value = expressionParser.Parse();
            tokenConsumer.Consume(TokenType.SEMICOLON);
        }

        return new VariableDeclaration(type, name.Content, value);
    }
}