using ABJAD.ParseEngine.Shared;
using ABJAD.ParseEngine.Statements;
using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Declarations;

public class ParseConstructorDeclarationStrategy : ParseDeclarationStrategy
{
    private readonly ITokenConsumer tokenConsumer;
    private readonly IParameterListConsumer parameterListConsumer;
    private readonly BlockStatementParser blockStatementParser;

    public ParseConstructorDeclarationStrategy(ITokenConsumer tokenConsumer,
        BlockStatementParser blockStatementParser,
        IParameterListConsumer parameterListConsumer)
    {
        Guard.Against.Null(tokenConsumer);
        Guard.Against.Null(parameterListConsumer);
        Guard.Against.Null(blockStatementParser);

        this.tokenConsumer = tokenConsumer;
        this.parameterListConsumer = parameterListConsumer;
        this.blockStatementParser = blockStatementParser;
    }

    public Declaration Parse()
    {
        tokenConsumer.Consume(TokenType.CONSTRUCTOR);
        tokenConsumer.Consume(TokenType.OPEN_PAREN);
        var parameters = parameterListConsumer.Consume();
        tokenConsumer.Consume(TokenType.CLOSE_PAREN);
        var body = blockStatementParser.Parse();
        return new ConstructorDeclaration(parameters, body as BlockStatement);
    }
}