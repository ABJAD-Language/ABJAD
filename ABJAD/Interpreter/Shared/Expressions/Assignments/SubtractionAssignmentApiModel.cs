namespace ABJAD.Interpreter.Shared.Expressions.Assignments;

public class SubtractionAssignmentApiModel
{
    public string Target { get; }
    public object Value { get; }

    public SubtractionAssignmentApiModel(string target, object value)
    {
        Target = target;
        Value = value;
    }
}