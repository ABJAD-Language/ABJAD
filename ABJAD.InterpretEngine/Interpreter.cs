using ABJAD.InterpretEngine.Shared;

namespace ABJAD.InterpretEngine;

public interface Interpreter<in T> where T : Binding
{
    void Interpret(T target);
}