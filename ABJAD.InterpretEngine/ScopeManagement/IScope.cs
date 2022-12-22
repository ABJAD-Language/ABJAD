using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.ScopeManagement;

public interface IScope
{
    bool ReferenceExists(string name);
    DataType GetType(string name);
    object Get(string name);
    void Set(string name, object value);
    void DefineVariable(string name, DataType type, object value);
    void DefineConstant(string name, DataType type, object value);
    bool IsConstant(string name);
    IScope Clone();
}