using ABJAD.Parser.Expressions;

namespace ABJAD.Parser.Statements;

public class IfStatementApiModel : StatementApiModel
{
    public ExpressionApiModel Condition { get; }
    public StatementApiModel Body { get; }

    public IfStatementApiModel(ExpressionApiModel condition, StatementApiModel body)
    {
        Condition = condition;
        Body = body;
        Type = "statement.if";
    }
}