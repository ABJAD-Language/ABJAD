namespace ABJAD.Parser.Expressions.Assignments;

public class DivisionAssignmentExpressionApiModel : ExpressionApiModel
{
    public string Target { get; }
    public ExpressionApiModel Value { get; }

    public DivisionAssignmentExpressionApiModel(string target, ExpressionApiModel value)
    {
        Target = target;
        Value = value;
        Type = "expression.assignment.division";
    }
}