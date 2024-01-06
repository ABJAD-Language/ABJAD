using ABJAD.Interpreter.Domain.Shared.Expressions;

namespace ABJAD.Interpreter.Domain.Shared.Statements;

public class Return : Statement
{
    public Expression? Target { get; set; }
}