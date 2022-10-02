using ABJAD.ParseEngine.Shared;
using ABJAD.ParseEngine.Statements;
using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Declarations;

public class ParseClassDeclarationStrategy : ParseDeclarationStrategy
{
    private readonly ITokenConsumer tokenConsumer;
    private readonly ParseBlockStatementStrategy blockStatementParser;

    public ParseClassDeclarationStrategy(ITokenConsumer tokenConsumer, ParseBlockStatementStrategy blockStatementParser)
    {
        Guard.Against.Null(tokenConsumer);
        Guard.Against.Null(blockStatementParser);
        this.tokenConsumer = tokenConsumer;
        this.blockStatementParser = blockStatementParser;
    }

    public Declaration Parse()
    {
        tokenConsumer.Consume(TokenType.CLASS);
        var nameId = tokenConsumer.Consume(TokenType.ID);
        var body = blockStatementParser.Parse();
        return new ClassDeclaration(nameId.Content, body as BlockStatement);
    }
}