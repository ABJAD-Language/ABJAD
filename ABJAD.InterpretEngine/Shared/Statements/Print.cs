using ABJAD.InterpretEngine.Shared.Expressions;

namespace ABJAD.InterpretEngine.Shared.Statements;

public class Print : Statement
{
    public Expression Target { get; set; }
}