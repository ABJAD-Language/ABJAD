namespace ABJAD.Parser.Domain.Statements;

public interface IStatementStrategyFactory
{
    ParseStatementStrategy Get();
}