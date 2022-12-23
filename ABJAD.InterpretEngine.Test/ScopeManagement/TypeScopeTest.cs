using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Declarations;

namespace ABJAD.InterpretEngine.Test.ScopeManagement;

public class TypeScopeTest
{
    [Fact(DisplayName = "returns false when asked about a type that does not exist")]
    public void returns_false_when_asked_about_a_type_that_does_not_exist()
    {
        var typeScope = new TypeScope(new Dictionary<string, ClassElement>());
        Assert.False(typeScope.TypeExists("type"));
    }

    [Fact(DisplayName = "returns true when asked about a type that exists")]
    public void returns_true_when_asked_about_a_type_that_exists()
    {
        var typeScope = new TypeScope(new Dictionary<string, ClassElement> { { "type", new ClassElement() }});
        Assert.True(typeScope.TypeExists("type"));
    }

    [Fact(DisplayName = "returns the specified type when asked about")]
    public void returns_the_specified_type_when_asked_about()
    {
        var classElement = new ClassElement { Declarations = new List<Declaration>() };
        var typeScope = new TypeScope(new Dictionary<string, ClassElement> { { "type", classElement }});
        Assert.Equal(classElement, typeScope.Get("type"));
    }

    [Fact(DisplayName = "defining a new type adds it to the state")]
    public void defining_a_new_type_adds_it_to_the_state()
    {
        var typeScope = new TypeScope(new Dictionary<string, ClassElement>());
        var classElement = new ClassElement();
        typeScope.Define("type", classElement);
        Assert.True(typeScope.TypeExists("type"));
        Assert.Equal(classElement, typeScope.Get("type"));
    }

    [Fact(DisplayName = "cloning a scope returns a deep copy of it")]
    public void cloning_a_scope_returns_a_deep_copy_of_it()
    {
        var typeScope = new TypeScope(new Dictionary<string, ClassElement>());
        var clonedScope = typeScope.Clone();
        typeScope.Define("type1", new ClassElement());
        Assert.False(clonedScope.TypeExists("type1"));
        clonedScope.Define("type2", new ClassElement());
        Assert.False(typeScope.TypeExists("type2"));
    }
}