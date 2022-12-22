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

    [Fact(DisplayName = "returns true if asked about a reference existence in the current scope which exists")]
    public void returns_true_if_asked_about_a_reference_existence_in_the_current_scope_which_exists()
    {
        var firstScope = new Scope(new Dictionary<string, StateElement>() { { "id1", new StateElement() }});
        var secondScope = new Scope(new Dictionary<string, StateElement>() { { "id2", new StateElement() }});
        var scopes = new List<IScope>() { firstScope, secondScope };
        var environment = new Environment(scopes);
        Assert.True(environment.ReferenceExistsInCurrentScope("id2"));
    }

    [Fact(DisplayName = "returns false if asked about a reference existence in the current scope which does not exist")]
    public void returns_false_if_asked_about_a_reference_existence_in_the_current_scope_which_does_not_exist()
    {
        var firstScope = new Scope(new Dictionary<string, StateElement>() { { "id1", new StateElement() }});
        var secondScope = new Scope(new Dictionary<string, StateElement>() { { "id2", new StateElement() }});
        var scopes = new List<IScope>() { firstScope, secondScope };
        var environment = new Environment(scopes);
        Assert.False(environment.ReferenceExistsInCurrentScope("id1"));
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

    [Fact(DisplayName = "returns true if asked about a constant reference if is constant")]
    public void returns_true_if_asked_about_a_constant_reference_if_is_constant()
    {
        var scope = new Scope(new Dictionary<string, StateElement>() { { "id", new StateElement() { IsConstant = true } }});
        var scopes = new List<IScope>() { scope };
        var environment = new Environment(scopes);
        Assert.True(environment.IsConstant("id"));
    }

    [Fact(DisplayName = "returns false if asked about a variable reference if is constant")]
    public void returns_true_if_asked_about_a_variable_reference_if_is_constant()
    {
        var scope = new Scope(new Dictionary<string, StateElement>() { { "id", new StateElement() { IsConstant = false } }});
        var scopes = new List<IScope>() { scope };
        var environment = new Environment(scopes);
        Assert.False(environment.IsConstant("id"));
    }

    [Fact(DisplayName = "checking if reference is constant gives priority to the later scopes")]
    public void checking_if_reference_is_constant_gives_priority_to_the_later_scopes()
    {
        var firstScope = new Scope(new Dictionary<string, StateElement>() { { "id", new StateElement() { IsConstant = true } }});
        var secondScope = new Scope(new Dictionary<string, StateElement>() { { "id", new StateElement() { IsConstant = false } }});
        var scopes = new List<IScope>() { firstScope, secondScope };
        var environment = new Environment(scopes);
        Assert.False(environment.IsConstant("id"));
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

    [Fact(DisplayName = "defining a new variable adds it to the last scope")]
    public void defining_a_new_variable_adds_it_to_the_last_scope()
    {
        var firstScope = new Scope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = 1 } }});
        var secondScope = new Scope(new Dictionary<string, StateElement>());
        var scopes = new List<IScope>() { firstScope, secondScope };
        var environment = new Environment(scopes);
        environment.DefineVariable("id", DataType.String(), "hello");
        Assert.True(environment.GetType("id").IsString());
        Assert.Equal("hello", environment.Get("id"));
        Assert.False(environment.IsConstant("id"));
    }
    
    [Fact(DisplayName = "adding a new variable to a cloned environment does not add it to the original one")]
    public void adding_a_new_variable_to_a_cloned_environment_does_not_add_it_to_the_original_one()
    {
        var scope = new Scope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = 1 } }});
        var scopes = new List<IScope>() { scope };
        var environment = new Environment(scopes);

        var cloneEnvironment = (Environment) environment.CloneScope();
        cloneEnvironment.DefineVariable("id2", DataType.String(), "hello");
        Assert.True(cloneEnvironment.ReferenceExists("id2"));
        Assert.False(environment.ReferenceExists("id2"));
    }

    [Fact(DisplayName = "defining a new variable in an environment does not add it to its clone")]
    public void defining_a_new_variable_in_an_environment_does_not_add_it_to_its_clone()
    {
        var scope = new Scope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = 1 } }});
        var scopes = new List<IScope>() { scope };
        var environment = new Environment(scopes);
        var cloneEnvironment = (Environment) environment.CloneScope();

        environment.DefineVariable("id2", DataType.String(), "hello");
        Assert.False(cloneEnvironment.ReferenceExists("id2"));
        Assert.True(environment.ReferenceExists("id2"));
    }

    [Fact(DisplayName = "defining a new constant adds it to the last scope")]
    public void defining_a_new_constant_adds_it_to_the_last_scope()
    {
        var firstScope = new Scope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = 1 } }});
        var secondScope = new Scope(new Dictionary<string, StateElement>());
        var scopes = new List<IScope>() { firstScope, secondScope };
        var environment = new Environment(scopes);
        environment.DefineConstant("id", DataType.String(), "hello");
        Assert.True(environment.GetType("id").IsString());
        Assert.Equal("hello", environment.Get("id"));
        Assert.True(environment.IsConstant("id"));
    }

    [Fact(DisplayName = "adding a new constant to a cloned environment does not add it to the original one")]
    public void adding_a_new_constant_to_a_cloned_environment_does_not_add_it_to_the_original_one()
    {
        var scope = new Scope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = 1 } }});
        var scopes = new List<IScope>() { scope };
        var environment = new Environment(scopes);

        var cloneEnvironment = (Environment) environment.CloneScope();
        cloneEnvironment.DefineConstant("id2", DataType.String(), "hello");
        Assert.True(cloneEnvironment.ReferenceExists("id2"));
        Assert.False(environment.ReferenceExists("id2"));
    }
    
    [Fact(DisplayName = "defining a new constant in an environment does not add it to its clone")]
    public void defining_a_new_constant_in_an_environment_does_not_add_it_to_its_clone()
    {
        var scope = new Scope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = 1 } }});
        var scopes = new List<IScope>() { scope };
        var environment = new Environment(scopes);
        var cloneEnvironment = (Environment) environment.CloneScope();

        environment.DefineConstant("id2", DataType.String(), "hello");
        Assert.False(cloneEnvironment.ReferenceExists("id2"));
        Assert.True(environment.ReferenceExists("id2"));
    }
    
    [Fact(DisplayName = "modifying the value of a reference in a cloned environment does not modify it in the original one")]
    public void modifying_the_value_of_a_reference_in_a_cloned_environment_does_not_modify_it_in_the_original_one()
    {
        var scope = new Scope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = 1 } }});
        var scopes = new List<IScope>() { scope };
        var environment = new Environment(scopes);
        var cloneEnvironment = (Environment) environment.CloneScope();

        cloneEnvironment.Set("id", 2);
        Assert.Equal(1, environment.Get("id"));
        Assert.Equal(2, cloneEnvironment.Get("id"));
    }

    [Fact(DisplayName = "modifying the value of a reference in an environment does not modify it in its clone")]
    public void modifying_the_value_of_a_reference_in_an_environment_does_not_modify_it_in_its_clone()
    {
        var scope = new Scope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = 1 } }});
        var scopes = new List<IScope>() { scope };
        var environment = new Environment(scopes);
        var cloneEnvironment = (Environment) environment.CloneScope();

        environment.Set("id", 2);
        Assert.Equal(2, environment.Get("id"));
        Assert.Equal(1, cloneEnvironment.Get("id"));
    }
}