namespace ABJAD.Interpreter.Shared.Expressions.Assignments;

public class AdditionAssignmentApiModel
{
    public string Target { get; }
    public object Value { get; }

    public AdditionAssignmentApiModel(string target, object value)
    {
        Target = target;
        Value = value;
    }
}