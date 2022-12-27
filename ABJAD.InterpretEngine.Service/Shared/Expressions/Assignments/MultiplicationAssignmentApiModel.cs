namespace ABJAD.InterpretEngine.Service.Shared.Expressions.Assignments;

public class MultiplicationAssignmentApiModel
{
    public string Target { get; }
    public object Value { get; }

    public MultiplicationAssignmentApiModel(string target, object value)
    {
        Target = target;
        Value = value;
    }
}