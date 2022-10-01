using ABJAD.ParseEngine.Expressions;

namespace ABJAD.ParseEngine.Statements;

public class AssignmentStatement : Statement
{
    public AssignmentStatement(string target, Expression value)
    {
        Target = target;
        Value = value;
    }

    public string Target { get; }
    public Expression Value { get; }
}