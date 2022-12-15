namespace ABJAD.InterpretEngine.Shared.Expressions.Assignments;

public interface AssignmentExpression : Expression
{
    string GetTarget();
    Expression GetValue();
}