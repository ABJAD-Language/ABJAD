using ABJAD.Interpreter.Domain.Shared.Expressions;

namespace ABJAD.Interpreter.Domain.Shared.Statements;

public class Conditional
{
    public Expression Condition { get; set; }
    public Statement Body { get; set; }
}