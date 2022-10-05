using ABJAD.ParseEngine.Statements;
using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Bindings;

public class StatementBinding : Binding
{
    public StatementBinding(Statement statement)
    {
        Guard.Against.Null(statement);
        Statement = statement;
    }

    public Statement Statement { get; }
}