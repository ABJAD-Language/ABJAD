using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Statements;

public class StatementContext
{
    private readonly ParseStatementStrategy strategy;

    public StatementContext(ParseStatementStrategy strategy)
    {
        Guard.Against.Null(strategy);
        this.strategy = strategy;
    }

    public Statement ParseStatement()
    {
        return strategy.Parse();
    }
}