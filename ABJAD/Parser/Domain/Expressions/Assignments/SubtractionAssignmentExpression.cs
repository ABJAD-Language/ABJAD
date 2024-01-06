using ABJAD.Parser.Domain.Primitives;

namespace ABJAD.Parser.Domain.Expressions.Assignments;

public class SubtractionAssignmentExpression : BinaryAssignmentExpression
{
    public SubtractionAssignmentExpression(IdentifierPrimitive target, Expression value) : base(target, value)
    {
    }
}