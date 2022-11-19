using ABJAD.ParseEngine.Service.Expressions;

namespace ABJAD.ParseEngine.Service.Statements;

public class WhileStatementApiModel : StatementApiModel
{
    public ExpressionApiModel Condition { get; }
    public StatementApiModel Body { get; }

    public WhileStatementApiModel(ExpressionApiModel condition, StatementApiModel body)
    {
        Condition = condition;
        Body = body;
        Type = "statement.while";
    }
}