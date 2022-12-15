namespace ABJAD.InterpretEngine.Shared.Expressions.Assignments;

public class AdditionAssignment : AssignmentExpression
{
    public string Target { get; init; }
    public Expression Value { get; init; }
    
    public string GetTarget()
    {
        return Target;
    }

    public Expression GetValue()
    {
        return Value;
    }
}