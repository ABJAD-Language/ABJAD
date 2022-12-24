using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.ScopeManagement;

public interface ITypeScope
{
    bool TypeExists(string name);
    bool HasConstructor(string className, params DataType[] parameterTypes);
    ClassElement Get(string name);
    void DefineConstructor(string className, ConstructorElement constructor);
    void Define(string name, ClassElement @class);
    ITypeScope Clone();
}