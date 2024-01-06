using ABJAD.Interpreter.Domain.Shared.Expressions;

namespace ABJAD.Interpreter.Domain.Shared.Statements;

public class ExpressionStatement : Statement
{
    public Expression Target { get; set; }
}