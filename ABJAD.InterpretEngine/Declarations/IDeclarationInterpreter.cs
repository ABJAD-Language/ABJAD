using ABJAD.InterpretEngine.Shared.Declarations;

namespace ABJAD.InterpretEngine.Declarations;

public interface IDeclarationInterpreter
{
    void Interpret(Declaration target);
}