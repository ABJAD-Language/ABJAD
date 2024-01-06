using ABJAD.Parser.Domain.Primitives;

namespace ABJAD.Parser.Domain.Expressions.Assignments;

public class AdditionAssignmentExpression : BinaryAssignmentExpression
{
    public AdditionAssignmentExpression(IdentifierPrimitive target, Expression value) : base(target, value)
    {
    }
}