using ABJAD.Parser.Domain.Bindings;
using ABJAD.Parser.Domain.Declarations;
using ABJAD.Parser.Domain.Expressions;
using ABJAD.Parser.Domain.Shared;
using Ardalis.GuardClauses;

namespace ABJAD.Parser.Domain.Statements;

public class StatementStrategyFactory : IStatementStrategyFactory
{
    private readonly ITokenConsumer tokenConsumer;

    public StatementStrategyFactory(ITokenConsumer tokenConsumer)
    {
        Guard.Against.Null(tokenConsumer);
        this.tokenConsumer = tokenConsumer;
    }

    public ParseStatementStrategy Get()
    {
        return GetHeadTokenType() switch
        {
            TokenType.PRINT => GetParsePrintStatementStrategy(),
            TokenType.RETURN => GetParseReturnStatementStrategy(),
            TokenType.OPEN_BRACE => GetParseBlockStatementStrategy(),
            TokenType.FOR => GetParseForStatementStrategy(),
            TokenType.WHILE => GetParseWhileStatementStrategy(),
            TokenType.IF => GetParseIfElseStatementStrategy(),
            TokenType.ID => GetAssignmentOrExpressionStatementStrategy(),
            _ => GetParseExpressionStatementStrategy()
        };
    }

    private ParseExpressionStatementStrategy GetParseExpressionStatementStrategy()
    {
        return new ParseExpressionStatementStrategy(tokenConsumer, ExpressionParserFactory.Get(tokenConsumer));
    }

    private ParseIfElseStatementStrategy GetParseIfElseStatementStrategy()
    {
        return new ParseIfElseStatementStrategy(tokenConsumer, ExpressionParserFactory.Get(tokenConsumer),
            new StatementStrategyFactory(tokenConsumer));
    }

    private ParseWhileStatementStrategy GetParseWhileStatementStrategy()
    {
        return new ParseWhileStatementStrategy(tokenConsumer, ExpressionParserFactory.Get(tokenConsumer), this);
    }

    private ParseForStatementStrategy GetParseForStatementStrategy()
    {
        return new ParseForStatementStrategy(tokenConsumer, ExpressionParserFactory.Get(tokenConsumer),
            GetBindingFactory(), this);
    }

    private ParseBlockStatementStrategy GetParseBlockStatementStrategy()
    {
        return new ParseBlockStatementStrategy(tokenConsumer, GetBindingFactory());
    }

    private ParseReturnStatementStrategy GetParseReturnStatementStrategy()
    {
        return new ParseReturnStatementStrategy(tokenConsumer, ExpressionParserFactory.Get(tokenConsumer));
    }

    private ParsePrintStatementStrategy GetParsePrintStatementStrategy()
    {
        return new ParsePrintStatementStrategy(tokenConsumer, ExpressionParserFactory.Get(tokenConsumer));
    }

    private BindingFactory GetBindingFactory()
    {
        return new BindingFactory(tokenConsumer, new DeclarationStrategyFactory(tokenConsumer), this);
    }

    private ParseStatementStrategy GetAssignmentOrExpressionStatementStrategy()
    {
        if (tokenConsumer.LookAhead(1).Type == TokenType.EQUAL)
        {
            return new ParseAssignmentStatementStrategy(tokenConsumer, ExpressionParserFactory.Get(tokenConsumer));
        }

        return GetParseExpressionStatementStrategy();
    }

    private TokenType GetHeadTokenType()
    {
        return tokenConsumer.Peek().Type;
    }
}