using ABJAD.ParseEngine.Primitives;

namespace ABJAD.ParseEngine.Expressions.Assignments;

public class MultiplicationAssignmentExpression : BinaryAssignmentExpression
{
    public MultiplicationAssignmentExpression(IdentifierPrimitive target, Expression value) : base(target, value)
    {
    }
}