namespace ABJAD.InterpretEngine.Service.Shared.Statements;

public class ExpressionStatementApiModel
{
    public object Expression { get; }

    public ExpressionStatementApiModel(object expression)
    {
        Expression = expression;
    }
}