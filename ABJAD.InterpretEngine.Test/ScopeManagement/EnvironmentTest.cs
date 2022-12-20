using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Types;
using Environment = ABJAD.InterpretEngine.ScopeManagement.Environment;

namespace ABJAD.InterpretEngine.Test.ScopeManagement;

public class EnvironmentTest
{
    [Fact(DisplayName = "returns false when asked about a reference that does not exist in any of the scopes")]
    public void returns_false_when_asked_about_a_reference_that_does_not_exist_in_any_of_the_scopes()
    {
        var firstScope = new Scope(new Dictionary<string, StateElement>() { { "id1", new StateElement() }});
        var secondScope = new Scope(new Dictionary<string, StateElement>() { { "id2", new StateElement() }});
        var scopes = new List<IScope>() { firstScope, secondScope };
        var environment = new Environment(scopes);
        Assert.False(environment.ReferenceExists("id3"));
    }

    [Fact(DisplayName = "returns true when asked about a reference that exists in one of the scopes")]
    public void returns_true_when_asked_about_a_reference_that_exists_in_one_of_the_scopes()
    {
        var firstScope = new Scope(new Dictionary<string, StateElement>() { { "id1", new StateElement() }});
        var secondScope = new Scope(new Dictionary<string, StateElement>() { { "id2", new StateElement() }});
        var scopes = new List<IScope>() { firstScope, secondScope };
        var environment = new Environment(scopes);
        Assert.True(environment.ReferenceExists("id2"));
    }

    [Fact(DisplayName = "getting the type of a reference gives priority to the state in the later scopes")]
    public void getting_the_type_of_a_reference_gives_priority_to_the_state_in_the_later_scopes()
    {
        var firstScope = new Scope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Type = DataType.Number() } }});
        var secondScope = new Scope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Type = DataType.String() } }});
        var scopes = new List<IScope>() { firstScope, secondScope };
        var environment = new Environment(scopes);
        Assert.False(environment.GetType("id").IsNumber());
        Assert.True(environment.GetType("id").IsString());
    }

    [Fact(DisplayName = "retrieving the value of a reference gives priority to the state in the later scopes")]
    public void retrieving_the_value_of_a_reference_gives_priority_to_the_state_in_the_later_scopes()
    {
        var firstScope = new Scope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = 1 } }});
        var secondScope = new Scope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = 2 } }});
        var scopes = new List<IScope>() { firstScope, secondScope };
        var environment = new Environment(scopes);
        Assert.Equal(2, environment.Get("id"));
    }

    [Fact(DisplayName = "setting a value to a reference should preserve its type when it exists in the last scope")]
    public void setting_a_value_to_a_reference_should_preserve_its_type_when_it_exists_in_the_last_scope()
    {
        var firstScope = new Scope(new Dictionary<string, StateElement>());
        var secondScope = new Scope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Type = DataType.Number(), Value = 1 } }});
        var scopes = new List<IScope>() { firstScope, secondScope };
        var environment = new Environment(scopes);
        environment.Set("id", 5);
        Assert.True(environment.GetType("id").IsNumber());
        Assert.Equal(5, environment.Get("id"));
    }

    [Fact(DisplayName = "setting a value to a reference should preserve its type when it exists in other than the last scope")]
    public void setting_a_value_to_a_reference_should_preserve_its_type_when_it_exists_in_other_than_the_last_scope()
    {
        var firstScope = new Scope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Type = DataType.Number(), Value = 1 } }});
        var secondScope = new Scope(new Dictionary<string, StateElement>());
        var scopes = new List<IScope>() { firstScope, secondScope };
        var environment = new Environment(scopes);
        environment.Set("id", 5);
        Assert.True(environment.GetType("id").IsNumber());
        Assert.Equal(5, environment.Get("id"));
    }

    [Fact(DisplayName = "defining a new value adds it to the last scope")]
    public void defining_a_new_value_adds_it_to_the_last_scope()
    {
        var firstScope = new Scope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = 1 } }});
        var secondScope = new Scope(new Dictionary<string, StateElement>());
        var scopes = new List<IScope>() { firstScope, secondScope };
        var environment = new Environment(scopes);
        environment.Define("id", DataType.String(), "hello");
        Assert.True(environment.GetType("id").IsString());
        Assert.Equal("hello", environment.Get("id"));
    }
}