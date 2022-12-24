using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.ScopeManagement;

public interface ScopeFacade
{
    bool ReferenceExists(string name);
    bool ReferenceExistsInCurrentScope(string name);
    DataType GetReferenceType(string name);
    object GetReference(string name);
    void SetReference(string name, object value);
    void DefineVariable(string name, DataType type, object value);
    void DefineConstant(string name, DataType type, object value);
    bool IsReferenceConstant(string name);
    
    bool FunctionExists(string name, int numberOfParameters);
    bool FunctionExistsInCurrentScope(string name, int numberOfParameters);
    DataType? GetFunctionReturnType(string name, int numberOfParameters);
    FunctionElement GetFunction(string name, int numberOfParameters);
    void DefineFunction(string name, FunctionElement function);
    
    bool TypeExists(string name);
    bool TypeHasConstructor(string className, params DataType[] parameterTypes);
    ClassElement GetType(string name);
    void DefineType(string name, ClassElement @class);
    void DefineTypeConstructor(string className, ConstructorElement constructor);

    ScopeFacade CloneScope();
    void AddNewScope();
}