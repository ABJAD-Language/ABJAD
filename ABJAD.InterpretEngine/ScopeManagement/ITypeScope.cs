namespace ABJAD.InterpretEngine.ScopeManagement;

public interface ITypeScope
{
    bool TypeExists(string name);
    ClassElement Get(string name);
    void Define(string name, ClassElement @class);
    ITypeScope Clone();
}