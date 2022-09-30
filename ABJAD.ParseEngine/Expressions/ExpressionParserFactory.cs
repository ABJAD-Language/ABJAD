namespace ABJAD.ParseEngine.Expressions;

public class ExpressionParserFactory
{
    public virtual ExpressionParser CreateInstance(ITokenConsumer consumer)
    {
        return new AbstractSyntaxTreeExpressionParser(consumer);
    }
}