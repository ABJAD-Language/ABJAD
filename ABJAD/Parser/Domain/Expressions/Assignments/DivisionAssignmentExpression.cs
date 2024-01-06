using ABJAD.Parser.Domain.Primitives;

namespace ABJAD.Parser.Domain.Expressions.Assignments;

public class DivisionAssignmentExpression : BinaryAssignmentExpression
{
    public DivisionAssignmentExpression(IdentifierPrimitive target, Expression value) : base(target, value)
    {
    }
}