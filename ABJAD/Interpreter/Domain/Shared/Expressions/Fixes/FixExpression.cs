namespace ABJAD.Interpreter.Domain.Shared.Expressions.Fixes;

public abstract class FixExpression : Expression
{
    public string Target { get; set; }
}