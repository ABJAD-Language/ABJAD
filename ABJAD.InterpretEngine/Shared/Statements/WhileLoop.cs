using ABJAD.InterpretEngine.Shared.Expressions;

namespace ABJAD.InterpretEngine.Shared.Statements;

public class WhileLoop : Statement
{
    public Expression Condition { get; set; }
    public Statement Body { get; set; }
}