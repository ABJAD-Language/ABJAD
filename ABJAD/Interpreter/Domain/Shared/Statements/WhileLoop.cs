using ABJAD.Interpreter.Domain.Shared.Expressions;

namespace ABJAD.Interpreter.Domain.Shared.Statements;

public class WhileLoop : Statement
{
    public Expression Condition { get; set; }
    public Statement Body { get; set; }
}