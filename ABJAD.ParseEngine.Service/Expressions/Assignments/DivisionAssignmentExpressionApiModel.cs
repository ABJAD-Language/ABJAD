using ABJAD.ParseEngine.Service.Primitives;

namespace ABJAD.ParseEngine.Service.Expressions.Assignments;

public class DivisionAssignmentExpressionApiModel : ExpressionApiModel
{
    public IdentifierPrimitiveApiModel Target { get; }
    public ExpressionApiModel Value { get; }

    public DivisionAssignmentExpressionApiModel(IdentifierPrimitiveApiModel target, ExpressionApiModel value)
    {
        Target = target;
        Value = value;
        Type = "expression.assignment.division";
    }
}