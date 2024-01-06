namespace ABJAD.Interpreter.Core;

public interface InterpreterService
{
    string Interpret(List<object> bindings);
}