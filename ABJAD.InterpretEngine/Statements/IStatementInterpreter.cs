using ABJAD.InterpretEngine.Shared.Statements;

namespace ABJAD.InterpretEngine.Statements;

public interface IStatementInterpreter
{
    StatementInterpretationResult Interpret(Statement target, bool functionContext = false);
}