using ABJAD.Parser.Domain.Statements;
using Ardalis.GuardClauses;

namespace ABJAD.Parser.Domain.Bindings;

public class StatementBinding : Binding
{
    public StatementBinding(Statement statement)
    {
        Guard.Against.Null(statement);
        Statement = statement;
    }

    public Statement Statement { get; }
}