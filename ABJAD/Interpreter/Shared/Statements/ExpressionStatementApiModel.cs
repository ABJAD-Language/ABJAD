namespace ABJAD.Interpreter.Shared.Statements;

public class ExpressionStatementApiModel
{
    public object Expression { get; }

    public ExpressionStatementApiModel(object expression)
    {
        Expression = expression;
    }
}