using ABJAD.InterpretEngine.Shared.Declarations;

namespace ABJAD.InterpretEngine.ScopeManagement;

public class ClassElement
{
    public List<Declaration> Declarations { get; set; }
    public List<ConstructorElement> Constructors { get; } = new List<ConstructorElement>();
}