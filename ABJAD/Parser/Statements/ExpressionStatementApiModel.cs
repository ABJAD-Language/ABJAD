using ABJAD.Parser.Expressions;

namespace ABJAD.Parser.Statements;

public class ExpressionStatementApiModel : StatementApiModel
{
    public ExpressionApiModel Expression { get; } // TODO refactor this level out
    public ExpressionStatementApiModel(ExpressionApiModel expression)
    {
        Expression = expression;
        Type = "statement.expression";
    }
}