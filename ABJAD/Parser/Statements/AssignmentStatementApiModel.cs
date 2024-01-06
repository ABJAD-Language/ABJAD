using ABJAD.Parser.Expressions;

namespace ABJAD.Parser.Statements;

public class AssignmentStatementApiModel : StatementApiModel
{
    public string Target { get; }
    public ExpressionApiModel Value { get; }

    public AssignmentStatementApiModel(string target, ExpressionApiModel value)
    {
        Target = target;
        Value = value;
        Type = "statement.assignment";
    }
}