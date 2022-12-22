using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.ScopeManagement;

public interface ScopeFacade
{
    bool ReferenceExists(string name);
    bool ReferenceExistsInCurrentScope(string name);
    DataType GetType(string name);
    object Get(string name);
    void Set(string name, object value);
    void DefineVariable(string name, DataType type, object value);
    void DefineConstant(string name, DataType type, object value);
    bool IsConstant(string name);
    ScopeFacade CloneScope();
    void AddNewScope();
}