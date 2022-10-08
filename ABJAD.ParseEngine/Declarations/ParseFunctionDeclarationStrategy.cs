using ABJAD.ParseEngine.Shared;
using ABJAD.ParseEngine.Statements;
using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Declarations;

public class ParseFunctionDeclarationStrategy : ParseDeclarationStrategy
{
    private readonly ITokenConsumer tokenConsumer;
    private readonly BlockStatementParser blockStatementParser;
    private readonly ITypeConsumer typeConsumer;
    private readonly IParameterListConsumer parameterListConsumer;

    public ParseFunctionDeclarationStrategy(ITokenConsumer tokenConsumer,
        BlockStatementParser blockStatementParser, ITypeConsumer typeConsumer,
        IParameterListConsumer parameterListConsumer)
    {
        Guard.Against.Null(tokenConsumer);
        Guard.Against.Null(blockStatementParser);
        Guard.Against.Null(typeConsumer);
        Guard.Against.Null(parameterListConsumer);
        this.tokenConsumer = tokenConsumer;
        this.blockStatementParser = blockStatementParser;
        this.typeConsumer = typeConsumer;
        this.parameterListConsumer = parameterListConsumer;
    }

    public Declaration Parse()
    {
        tokenConsumer.Consume(TokenType.FUNC);
        var name = tokenConsumer.Consume(TokenType.ID).Content;
        tokenConsumer.Consume(TokenType.OPEN_PAREN);

        var parameters = parameterListConsumer.Consume();

        tokenConsumer.Consume(TokenType.CLOSE_PAREN);
        tokenConsumer.Consume(TokenType.COLON);

        var returnType = typeConsumer.ConsumeTypeOrVoid();

        var body = blockStatementParser.Parse();

        return new FunctionDeclaration(name, returnType, parameters, body as BlockStatement);
    }
}