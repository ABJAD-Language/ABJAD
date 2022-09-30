using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Shared;

namespace ABJAD.ParseEngine.Statements;

public class ParsePrintStatementStrategy : ParseStatementStrategy
{
    private readonly ITokenConsumer consumer;
    private readonly ExpressionParserFactory expressionParserFactory;

    public ParsePrintStatementStrategy(ITokenConsumer tokenConsumer, ExpressionParserFactory expressionParserFactory)
    {
        this.expressionParserFactory = expressionParserFactory;
        consumer = tokenConsumer;
    }

    public Statement Parse()
    {
        consumer.Consume(TokenType.PRINT);
        consumer.Consume(TokenType.OPEN_PAREN);

        var expressionParser = expressionParserFactory.CreateInstance(consumer);
        var expression = expressionParser.Parse();

        consumer.Consume(TokenType.CLOSE_PAREN);
        consumer.Consume(TokenType.SEMICOLON);

        return new PrintStatement(expression);
    }
}