using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Shared;
using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Declarations;

public class ParseConstantDeclarationStrategy : ParseDeclarationStrategy
{
    private readonly ITokenConsumer tokenConsumer;
    private readonly ExpressionParser expressionParser;
    private readonly ITypeConsumer typeConsumer;

    public ParseConstantDeclarationStrategy(ITokenConsumer tokenConsumer, ExpressionParser expressionParser,
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
        tokenConsumer.Consume(TokenType.CONST);
        var type = typeConsumer.Consume();
        var name = tokenConsumer.Consume(TokenType.ID);

        tokenConsumer.Consume(TokenType.EQUAL);
        var value = expressionParser.Parse();
        tokenConsumer.Consume(TokenType.SEMICOLON);

        return new ConstantDeclaration(type.GetValue(), name.Content, value);
    }
}