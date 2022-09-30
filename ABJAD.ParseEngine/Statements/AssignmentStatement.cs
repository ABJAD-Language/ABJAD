using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Primitives;

namespace ABJAD.ParseEngine.Statements;

public class AssignmentStatement : Statement
{
    public AssignmentStatement(IdentifierPrimitive target, Expression value)
    {
        Target = target;
        Value = value;
    }

    public IdentifierPrimitive Target { get; }
    public Expression Value { get; }
}