using ABJAD.ParseEngine.Service.Primitives;

namespace ABJAD.ParseEngine.Service.Expressions.Assignments;

public class AdditionAssignmentExpressionApiModel : ExpressionApiModel
{
    public IdentifierPrimitiveApiModel Target { get; }
    public ExpressionApiModel Value { get; }

    public AdditionAssignmentExpressionApiModel(IdentifierPrimitiveApiModel target, ExpressionApiModel value)
    {
        Target = target;
        Value = value;
        Type = "expression.assignment.addition";
    }
}