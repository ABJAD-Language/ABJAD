using ABJAD.Interpreter.Domain.Shared.Expressions;

namespace ABJAD.Interpreter.Domain.Shared.Statements;

public class Assignment : Statement
{
    public string Target { get; init; }
    public Expression Value { get; init; }
}