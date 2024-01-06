using ABJAD.Parser.Domain.Primitives;

namespace ABJAD.Parser.Domain.Expressions.Assignments;

public abstract class BinaryAssignmentExpression : Expression
{
    protected BinaryAssignmentExpression(IdentifierPrimitive target, Expression value)
    {
        Target = target;
        Value = value;
    }

    public IdentifierPrimitive Target { get; }
    public Expression Value { get; }
}