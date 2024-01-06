namespace ABJAD.Parser.Expressions.Assignments;

public class MultiplicationAssignmentExpressionApiModel : ExpressionApiModel
{
    public string Target { get; }
    public ExpressionApiModel Value { get; }

    public MultiplicationAssignmentExpressionApiModel(string target, ExpressionApiModel value)
    {
        Target = target;
        Value = value;
        Type = "expression.assignment.multiplication";
    }
}