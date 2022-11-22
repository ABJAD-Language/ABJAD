using ABJAD.ParseEngine.Service.Primitives;

namespace ABJAD.ParseEngine.Service.Expressions.Assignments;

public class SubtractionAssignmentExpressionApiModel : ExpressionApiModel
{
    public string Target { get; }
    public ExpressionApiModel Value { get; }

    public SubtractionAssignmentExpressionApiModel(string target, ExpressionApiModel value)
    {
        Target = target;
        Value = value;
        Type = "expression.assignment.subtraction";
    }
}