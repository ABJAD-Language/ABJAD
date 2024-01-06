using ABJAD.Parser.Domain.Expressions;

namespace ABJAD.Parser.Domain.Statements;

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