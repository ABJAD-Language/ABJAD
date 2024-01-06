using ABJAD.Interpreter.Domain.ScopeManagement;
using ABJAD.Interpreter.Domain.Shared.Declarations;
using ABJAD.Interpreter.Domain.Types;

namespace ABJAD.Test.Domain.ScopeManagement;

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
        var typeScope = new TypeScope(new Dictionary<string, ClassElement> { { "type", new ClassElement() } });
        Assert.True(typeScope.TypeExists("type"));
    }

    [Fact(DisplayName = "returns false when asked about a type that exists if it has constructor when it does not")]
    public void returns_false_when_asked_about_a_type_that_exists_if_it_has_constructor_when_it_does_not()
    {
        var typeScope = new TypeScope(new Dictionary<string, ClassElement> { { "type", new ClassElement() } });
        Assert.False(typeScope.HasConstructor("type"));
    }

    [Fact(DisplayName = "returns true when asked about a type that exists if it has a parameterless constructor when it does")]
    public void returns_true_when_asked_about_a_type_that_exists_if_it_has_a_parameterless_constructor_when_it_does()
    {
        var classElement = new ClassElement();
        classElement.Constructors.Add(new ConstructorElement() { Parameters = new List<FunctionParameter>() });
        var typeScope = new TypeScope(new Dictionary<string, ClassElement> { { "type", classElement } });
        Assert.True(typeScope.HasConstructor("type"));
    }

    [Fact(DisplayName = "returns false when asked about a type that exists if it has a constructor when a similar one with different order of parameters exists")]
    public void returns_false_when_asked_about_a_type_that_exists_if_it_has_a_constructor_when_a_similar_one_with_different_order_of_parameters_exists()
    {
        var classElement = new ClassElement();
        classElement.Constructors.Add(new ConstructorElement()
        {
            Parameters = new List<FunctionParameter>()
            {
                new() { Type = DataType.Bool() },
                new() { Type = DataType.Number() }
            }
        });
        var typeScope = new TypeScope(new Dictionary<string, ClassElement> { { "type", classElement } });
        Assert.False(typeScope.HasConstructor("type", DataType.Number(), DataType.Bool()));
    }

    [Fact(DisplayName = "returns true when asked about a type that exists if it has a specific constructor and a matching one exists")]
    public void returns_true_when_asked_about_a_type_that_exists_if_it_has_a_specific_constructor_and_a_matching_one_exists()
    {
        var classElement = new ClassElement();
        classElement.Constructors.Add(new ConstructorElement()
        {
            Parameters = new List<FunctionParameter>()
            {
                new() { Type = DataType.Bool() },
                new() { Type = DataType.Number() }
            }
        });
        var typeScope = new TypeScope(new Dictionary<string, ClassElement> { { "type", classElement } });
        Assert.True(typeScope.HasConstructor("type", DataType.Bool(), DataType.Number()));
    }

    [Fact(DisplayName = "returns the specified constructor when asked to do so")]
    public void returns_the_specified_constructor_when_asked_to_do_so()
    {
        var classElement = new ClassElement();
        var constructorElement = new ConstructorElement()
        {
            Parameters = new List<FunctionParameter>()
            {
                new() { Type = DataType.Bool() },
                new() { Type = DataType.Number() }
            }
        };
        classElement.Constructors.Add(constructorElement);
        var typeScope = new TypeScope(new Dictionary<string, ClassElement> { { "type", classElement } });
        Assert.Equal(constructorElement, typeScope.GetConstructor("type", DataType.Bool(), DataType.Number()));
    }

    [Fact(DisplayName = "returns the specified type when asked about")]
    public void returns_the_specified_type_when_asked_about()
    {
        var classElement = new ClassElement { Declarations = new List<Declaration>() };
        var typeScope = new TypeScope(new Dictionary<string, ClassElement> { { "type", classElement } });
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

    [Fact(DisplayName = "defining a constructor adds it to the type")]
    public void defining_a_constructor_adds_it_to_the_type()
    {
        var typeScope = new TypeScope(new Dictionary<string, ClassElement> { { "type", new ClassElement() } });
        var constructorElement = new ConstructorElement()
        {
            Parameters = new List<FunctionParameter>()
            {
                new() { Type = DataType.Bool() },
                new() { Type = DataType.Number() }
            }
        };
        typeScope.DefineConstructor("type", constructorElement);
        Assert.True(typeScope.HasConstructor("type", DataType.Bool(), DataType.Number()));
        Assert.Equal(constructorElement, typeScope.Get("type").Constructors[0]);
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

    [Fact(DisplayName = "aggregating two scopes squashes the second on top of the first")]
    public void aggregating_two_scopes_squashes_the_second_on_top_of_the_first()
    {
        var typeScope1 = new TypeScope(new Dictionary<string, ClassElement>());
        typeScope1.Define("type1", new ClassElement());
        var typeScope2 = new TypeScope(new Dictionary<string, ClassElement>());
        typeScope2.Define("type2", new ClassElement());

        var typeScope = typeScope1.Aggregate(typeScope2);
        Assert.True(typeScope.TypeExists("type1"));
        Assert.True(typeScope.TypeExists("type2"));
    }
}