using ABJAD.InterpretEngine.Shared.Statements;

namespace ABJAD.InterpretEngine.Statements;

public interface IStatementInterpreter
{
    void Interpret(Statement target, bool functionContext = false);
}