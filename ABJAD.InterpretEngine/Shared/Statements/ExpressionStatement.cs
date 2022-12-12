using ABJAD.InterpretEngine.Shared.Expressions;

namespace ABJAD.InterpretEngine.Shared.Statements;

public class ExpressionStatement : Statement
{
    public Expression Target { get; set; }
}