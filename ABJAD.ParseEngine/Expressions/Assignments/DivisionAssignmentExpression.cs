using ABJAD.ParseEngine.Primitives;

namespace ABJAD.ParseEngine.Expressions.Assignments;

public class DivisionAssignmentExpression : BinaryAssignmentExpression
{
    public DivisionAssignmentExpression(IdentifierPrimitive target, Expression value) : base(target, value)
    {
    }
}