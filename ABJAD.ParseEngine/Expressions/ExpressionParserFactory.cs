namespace ABJAD.ParseEngine.Expressions;

public static class ExpressionParserFactory
{
    public static ExpressionParser Get(ITokenConsumer tokenConsumer)
    {
        return new AbstractSyntaxTreeExpressionParser(tokenConsumer);
    }
}