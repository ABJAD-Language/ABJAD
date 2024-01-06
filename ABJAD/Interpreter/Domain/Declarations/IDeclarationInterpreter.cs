using ABJAD.Interpreter.Domain.Shared.Declarations;

namespace ABJAD.Interpreter.Domain.Declarations;

public interface IDeclarationInterpreter
{
    void Interpret(Declaration target);
}