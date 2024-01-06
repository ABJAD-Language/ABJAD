namespace ABJAD.Interpreter.Shared.Statements;

public class AssignmentApiModel
{
    public string Target { get; }
    public object Value { get; }

    public AssignmentApiModel(string target, object value)
    {
        Target = target;
        Value = value;
    }
}