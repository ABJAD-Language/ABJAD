using ABJAD.ParseEngine.Service.Expressions;

namespace ABJAD.ParseEngine.Service.Statements;

public class ExpressionStatementApiModel : StatementApiModel
{
    public ExpressionApiModel Expression { get; }
    public ExpressionStatementApiModel(ExpressionApiModel expression)
    {
        Expression = expression;
        Type = "statement.expression";
    }
}