using ABJAD.ParseEngine.Primitives;

namespace ABJAD.ParseEngine.Expressions.Assignments;

public class AdditionAssignmentExpression : BinaryAssignmentExpression
{
    public AdditionAssignmentExpression(IdentifierPrimitive target, Expression value) : base(target, value)
    {
    }
}