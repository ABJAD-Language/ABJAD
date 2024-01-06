using ABJAD.Interpreter.Domain.Shared.Declarations;

namespace ABJAD.Interpreter.Domain.ScopeManagement;

public class ClassElement
{
    public List<Declaration> Declarations { get; set; }
    public List<ConstructorElement> Constructors { get; } = new List<ConstructorElement>();
}