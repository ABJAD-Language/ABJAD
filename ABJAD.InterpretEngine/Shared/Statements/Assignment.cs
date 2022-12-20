using ABJAD.InterpretEngine.Shared.Expressions;

namespace ABJAD.InterpretEngine.Shared.Statements;

public class Assignment : Statement
{
    public string Target { get; init; }
    public Expression Value { get; init; }
}