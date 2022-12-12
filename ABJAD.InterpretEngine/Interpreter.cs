using ABJAD.InterpretEngine.Shared;

namespace ABJAD.InterpretEngine;

public interface Interpreter<in T>
{
    void Interpret(T target);
}