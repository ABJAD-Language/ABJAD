using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.ScopeManagement;

public interface IFunctionScope
{
    bool FunctionExists(string name, params DataType[] parametersTypes);
    DataType? GetFunctionReturnType(string name, params DataType[] parametersTypes);
    FunctionElement GetFunction(string name, params DataType[] parametersTypes);
    void DefineFunction(string name, FunctionElement function);
    IFunctionScope Clone();
}