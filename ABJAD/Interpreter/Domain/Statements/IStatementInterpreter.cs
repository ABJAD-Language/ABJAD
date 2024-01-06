using ABJAD.Interpreter.Domain.Shared.Statements;

namespace ABJAD.Interpreter.Domain.Statements;

public interface IStatementInterpreter
{
    StatementInterpretationResult Interpret(Statement target, bool functionContext = false);
}