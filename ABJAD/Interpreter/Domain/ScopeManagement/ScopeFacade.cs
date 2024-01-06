using ABJAD.Interpreter.Domain.Types;

namespace ABJAD.Interpreter.Domain.ScopeManagement;

public interface ScopeFacade
{
    bool ReferenceExists(string name);
    bool ReferenceExistsInCurrentScope(string name);
    DataType GetReferenceType(string name);
    object GetReference(string name);
    void UpdateReference(string name, object value);
    void DefineVariable(string name, DataType type, object value);
    void DefineConstant(string name, DataType type, object value);
    bool IsReferenceConstant(string name);

    bool FunctionExists(string name, params DataType[] parametersTypes);
    bool FunctionExistsInCurrentScope(string name, params DataType[] parametersTypes);
    DataType? GetFunctionReturnType(string name, params DataType[] parametersTypes);
    FunctionElement GetFunction(string name, params DataType[] parametersTypes);
    void DefineFunction(string name, FunctionElement function);

    bool TypeExists(string name);
    bool TypeHasConstructor(string className, params DataType[] parameterTypes);
    ClassElement GetType(string name);
    ConstructorElement GetTypeConstructor(string name, params DataType[] parameterTypes);
    void DefineType(string name, ClassElement @class);
    void DefineTypeConstructor(string className, ConstructorElement constructor);

    [Obsolete]
    ScopeFacade CloneScope();
    void AddNewScope();
    void RemoveLastScope();
    void AddScope(ScopeFacade scopeFacade);
    List<Scope> GetScopes();
}