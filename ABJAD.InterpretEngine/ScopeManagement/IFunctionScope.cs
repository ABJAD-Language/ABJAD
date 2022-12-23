using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.ScopeManagement;

public interface IFunctionScope
{
    bool FunctionExists(string name, int numberOfParameters);
    DataType? GetFunctionReturnType(string name, int numberOfParameters);
    FunctionElement GetFunction(string name, int numberOfParameters);
    void DefineFunction(string name, FunctionElement function);
    IFunctionScope Clone();
}