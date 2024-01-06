using ABJAD.Parser.Domain.Bindings;
using ABJAD.Parser.Domain.Expressions;
using ABJAD.Parser.Domain.Shared;
using ABJAD.Parser.Domain.Statements;
using Ardalis.GuardClauses;

namespace ABJAD.Parser.Domain.Declarations;

public class DeclarationStrategyFactory : IDeclarationStrategyFactory
{
    private readonly ITokenConsumer tokenConsumer;

    public DeclarationStrategyFactory(ITokenConsumer tokenConsumer)
    {
        Guard.Against.Null(tokenConsumer);
        this.tokenConsumer = tokenConsumer;
    }

    public ParseDeclarationStrategy Get()
    {
        return GetHeadTokenType() switch
        {
            TokenType.VAR => GetVariableStrategy(),
            TokenType.CONST => GetConstantStrategy(),
            TokenType.FUNC => GetFunctionStrategy(),
            TokenType.CONSTRUCTOR => GetConstructorStrategy(),
            TokenType.CLASS => GetClassStrategy()
        };
    }

    private TokenType GetHeadTokenType()
    {
        return tokenConsumer.Peek().Type;
    }

    private ParseClassDeclarationStrategy GetClassStrategy()
    {
        return new ParseClassDeclarationStrategy(tokenConsumer, GetBlockDeclarationParser());
    }

    private ParseBlockDeclarationStrategy GetBlockDeclarationParser()
    {
        return new ParseBlockDeclarationStrategy(tokenConsumer, new DeclarationStrategyFactory(tokenConsumer));
    }

    private ParseConstructorDeclarationStrategy GetConstructorStrategy()
    {
        return new ParseConstructorDeclarationStrategy(tokenConsumer, GetBlockStatementParser(),
            new ParameterListConsumer(tokenConsumer, new TypeConsumer(tokenConsumer)));
    }

    private ParseFunctionDeclarationStrategy GetFunctionStrategy()
    {
        return new ParseFunctionDeclarationStrategy(tokenConsumer, GetBlockStatementParser(),
            new TypeConsumer(tokenConsumer), new ParameterListConsumer(tokenConsumer, new TypeConsumer(tokenConsumer)));
    }

    private ParseConstantDeclarationStrategy GetConstantStrategy()
    {
        return new ParseConstantDeclarationStrategy(tokenConsumer, ExpressionParserFactory.Get(tokenConsumer),
            new TypeConsumer(tokenConsumer));
    }

    private ParseVariableDeclarationStrategy GetVariableStrategy()
    {
        return new ParseVariableDeclarationStrategy(tokenConsumer, ExpressionParserFactory.Get(tokenConsumer),
            new TypeConsumer(tokenConsumer));
    }

    private ParseBlockStatementStrategy GetBlockStatementParser()
    {
        return new ParseBlockStatementStrategy(tokenConsumer,
            new BindingFactory(tokenConsumer, this, new StatementStrategyFactory(tokenConsumer)));
    }
}