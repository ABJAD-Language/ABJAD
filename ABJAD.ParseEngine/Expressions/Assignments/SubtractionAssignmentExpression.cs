using ABJAD.ParseEngine.Primitives;

namespace ABJAD.ParseEngine.Expressions.Assignments;

public class SubtractionAssignmentExpression : BinaryAssignmentExpression
{
    public SubtractionAssignmentExpression(IdentifierPrimitive target, Expression value) : base(target, value)
    {
    }
}