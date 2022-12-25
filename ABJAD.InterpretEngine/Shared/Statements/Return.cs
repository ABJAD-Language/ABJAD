using ABJAD.InterpretEngine.Shared.Expressions;

namespace ABJAD.InterpretEngine.Shared.Statements;

public class Return : Statement
{
    public Expression? Target { get; set; }
}