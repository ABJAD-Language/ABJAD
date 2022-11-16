using ABJAD.ParseEngine.Service.Primitives;

namespace ABJAD.ParseEngine.Service.Expressions.Assignments;

public class MultiplicationAssignmentExpressionApiModel : ExpressionApiModel
{
    public IdentifierPrimitiveApiModel Target { get; }
    public ExpressionApiModel Value { get; }

    public MultiplicationAssignmentExpressionApiModel(IdentifierPrimitiveApiModel target, ExpressionApiModel value)
    {
        Target = target;
        Value = value;
        Type = "expression.assignment.multiplication";
    }
}