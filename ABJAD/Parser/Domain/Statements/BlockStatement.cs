using ABJAD.Parser.Domain.Bindings;
using Ardalis.GuardClauses;

namespace ABJAD.Parser.Domain.Statements;

public class BlockStatement : Statement
{
    public BlockStatement(List<Binding> bindings)
    {
        Guard.Against.Null(bindings);
        Bindings = bindings;
    }

    public List<Binding> Bindings { get; }
}