using ABJAD.Parser.Domain.Primitives;

namespace ABJAD.Parser.Domain.Expressions.Assignments;

public class MultiplicationAssignmentExpression : BinaryAssignmentExpression
{
    public MultiplicationAssignmentExpression(IdentifierPrimitive target, Expression value) : base(target, value)
    {
    }
}