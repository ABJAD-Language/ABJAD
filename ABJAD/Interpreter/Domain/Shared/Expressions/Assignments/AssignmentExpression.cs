namespace ABJAD.Interpreter.Domain.Shared.Expressions.Assignments;

public abstract class AssignmentExpression : Expression
{
    public string Target { get; init; }
    public Expression Value { get; init; }
}