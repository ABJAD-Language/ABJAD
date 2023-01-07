namespace ABJAD.InterpretEngine.Service.Core;

public interface InterpreterService
{
    string Interpret(List<object> bindings);
}