using ABJAD.InterpretEngine.Shared.Expressions;

namespace ABJAD.InterpretEngine.Shared.Statements;

public class Conditional
{
    public Expression Condition { get; set; }
    public Statement Body { get; set; }
}