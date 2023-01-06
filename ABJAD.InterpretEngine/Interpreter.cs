using ABJAD.InterpretEngine.Shared;

namespace ABJAD.InterpretEngine;

public interface Interpreter
{
    void Interpret(Binding target);
}