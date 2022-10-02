using ABJAD.ParseEngine.Shared;
using ABJAD.ParseEngine.Statements;
using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Declarations;

public class ParseConstructorDeclarationStrategy : ParseDeclarationStrategy
{
    private readonly ITokenConsumer tokenConsumer;
    private readonly ITypeConsumer typeConsumer;
    private readonly IParameterListConsumer parameterListConsumer;
    private readonly ParseBlockStatementStrategy blockStatementParser;

    public ParseConstructorDeclarationStrategy(ITokenConsumer tokenConsumer, ITypeConsumer typeConsumer,
        IParameterListConsumer parameterListConsumer, ParseBlockStatementStrategy blockStatementParser)
    {
        Guard.Against.Null(tokenConsumer);
        Guard.Against.Null(typeConsumer);
        Guard.Against.Null(parameterListConsumer);
        Guard.Against.Null(blockStatementParser);

        this.tokenConsumer = tokenConsumer;
        this.typeConsumer = typeConsumer;
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