using ABJAD.Interpreter.Domain.Shared;

namespace ABJAD.Interpreter.Domain;

public interface Interpreter
{
    void Interpret(Binding target);
}