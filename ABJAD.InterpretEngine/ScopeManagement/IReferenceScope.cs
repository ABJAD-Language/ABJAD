using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.ScopeManagement;

public interface IReferenceScope
{
    bool ReferenceExists(string name);
    DataType GetType(string name);
    object Get(string name);
    void Update(string name, object value);
    void DefineVariable(string name, DataType type, object value);
    void DefineConstant(string name, DataType type, object value);
    bool IsConstant(string name);
    IReferenceScope Clone();
    IReferenceScope Aggregate(IReferenceScope referenceScope);
}