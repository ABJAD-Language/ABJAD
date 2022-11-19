using ABJAD.ParseEngine.Service.Expressions;

namespace ABJAD.ParseEngine.Service.Statements;

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