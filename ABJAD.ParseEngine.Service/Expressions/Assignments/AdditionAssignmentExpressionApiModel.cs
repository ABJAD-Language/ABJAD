namespace ABJAD.ParseEngine.Service.Expressions.Assignments;

public class AdditionAssignmentExpressionApiModel : ExpressionApiModel
{
    public string Target { get; }
    public ExpressionApiModel Value { get; }

    public AdditionAssignmentExpressionApiModel(string target, ExpressionApiModel value)
    {
        Target = target;
        Value = value;
        Type = "expression.assignment.addition";
    }
}