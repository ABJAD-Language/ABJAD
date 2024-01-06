namespace ABJAD.Interpreter.Shared.Expressions.Assignments;

public class DivisionAssignmentApiModel
{
    public string Target { get; }
    public object Value { get; }

    public DivisionAssignmentApiModel(string target, object value)
    {
        Target = target;
        Value = value;
    }
}