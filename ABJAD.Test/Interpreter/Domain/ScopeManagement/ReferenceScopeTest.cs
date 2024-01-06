using ABJAD.Interpreter.Domain.ScopeManagement;
using ABJAD.Interpreter.Domain.Types;
using NSubstitute;

namespace ABJAD.Test.Interpreter.Domain.ScopeManagement;

public class ReferenceScopeTest
{
    [Fact(DisplayName = "returns false when asked about a reference that does not exist")]
    public void returns_false_when_asked_about_a_reference_that_does_not_exist()
    {
        var scope = new ReferenceScope(new Dictionary<string, StateElement>());
        Assert.False(scope.ReferenceExists("id"));
    }

    [Fact(DisplayName = "returns true when asked about a reference that exists")]
    public void returns_true_when_asked_about_a_reference_that_exists()
    {
        var scope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id", new StateElement() } });
        Assert.True(scope.ReferenceExists("id"));
    }

    [Fact(DisplayName = "returns the type of the state element when asked about it")]
    public void returns_the_type_of_the_state_element_when_asked_about_it()
    {
        var dataType = Substitute.For<DataType>();
        var state = new Dictionary<string, StateElement>() { { "id", new StateElement() { Type = dataType } } };
        var scope = new ReferenceScope(state);
        Assert.Equal(dataType, scope.GetType("id"));
    }

    [Fact(DisplayName = "returns the value of the state element when asked about it")]
    public void returns_the_value_of_the_state_element_when_asked_about_it()
    {
        var dataValue = new object();
        var state = new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = dataValue } } };
        var scope = new ReferenceScope(state);
        Assert.Equal(dataValue, scope.Get("id"));
    }

    [Fact(DisplayName = "returns true when asked about a constant that exists")]
    public void returns_true_when_asked_about_a_constant_that_exists()
    {
        var dataValue = new object();
        var state = new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = dataValue, IsConstant = true } } };
        var scope = new ReferenceScope(state);
        Assert.True(scope.IsConstant("id"));
    }

    [Fact(DisplayName = "updates the value of a state reference correctly")]
    public void updates_the_value_of_a_state_reference_correctly()
    {
        var state = new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = 1 } } };
        var scope = new ReferenceScope(state);
        scope.Update("id", 2);
        Assert.Equal(2, scope.Get("id"));
    }

    [Fact(DisplayName = "fails to update the value of a state element that does not exist")]
    public void fails_to_update_the_value_of_a_state_element_that_does_not_exist()
    {
        var scope = new ReferenceScope(new Dictionary<string, StateElement>());
        Assert.Throws<KeyNotFoundException>(() => scope.Update("id", 2));
    }

    [Fact(DisplayName = "fails to update the value of a state element that is constant")]
    public void fails_to_update_the_value_of_a_state_element_that_is_constant()
    {
        var state = new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = 1, IsConstant = true } } };
        var scope = new ReferenceScope(state);
        Assert.Throws<IllegalConstantValueChangeException>(() => scope.Update("id", 2));
    }

    [Fact(DisplayName = "adds a new state element to the state when defining a new variable reference")]
    public void adds_a_new_state_element_to_the_state_when_defining_a_new_variable_reference()
    {
        var scope = new ReferenceScope(new Dictionary<string, StateElement>());
        scope.DefineVariable("id", DataType.Bool(), true);
        Assert.True(scope.ReferenceExists("id"));
        Assert.True(scope.GetType("id").IsBool());
        Assert.Equal(true, scope.Get("id"));
        Assert.False(scope.IsConstant("id"));
    }

    [Fact(DisplayName = "adds a new state element to the state when defining a new constant reference")]
    public void adds_a_new_state_element_to_the_state_when_defining_a_new_constant_reference()
    {
        var scope = new ReferenceScope(new Dictionary<string, StateElement>());
        scope.DefineConstant("id", DataType.Bool(), true);
        Assert.True(scope.ReferenceExists("id"));
        Assert.True(scope.GetType("id").IsBool());
        Assert.Equal(true, scope.Get("id"));
        Assert.True(scope.IsConstant("id"));
    }

    [Fact(DisplayName = "fails to define a new variable with the same name as another existing one")]
    public void fails_to_define_a_new_variable_with_the_same_name_as_another_existing_one()
    {
        var state = new Dictionary<string, StateElement>() { { "id", new StateElement() } };
        var scope = new ReferenceScope(state);
        Assert.Throws<ArgumentException>(() => scope.DefineVariable("id", DataType.Bool(), true));
    }

    [Fact(DisplayName = "fails to define a new constant with the same name as another existing one")]
    public void fails_to_define_a_new_constant_with_the_same_name_as_another_existing_one()
    {
        var state = new Dictionary<string, StateElement>() { { "id", new StateElement() } };
        var scope = new ReferenceScope(state);
        Assert.Throws<ArgumentException>(() => scope.DefineConstant("id", DataType.Bool(), true));
    }

    [Fact(DisplayName = "adding new variable to a cloned scope does not add it to the original one")]
    public void adding_new_variable_to_a_cloned_scope_does_not_add_it_to_the_original_one()
    {
        var scope = new ReferenceScope(new Dictionary<string, StateElement>());
        var clonedScope = scope.Clone();
        clonedScope.DefineVariable("id", DataType.Number(), 1);
        Assert.False(scope.ReferenceExists("id"));
        Assert.True(clonedScope.ReferenceExists("id"));
    }

    [Fact(DisplayName = "adding new constant to a cloned scope does not add it to the original one")]
    public void adding_new_constant_to_a_cloned_scope_does_not_add_it_to_the_original_one()
    {
        var scope = new ReferenceScope(new Dictionary<string, StateElement>());
        var clonedScope = scope.Clone();
        clonedScope.DefineConstant("id", DataType.Number(), 1);
        Assert.False(scope.ReferenceExists("id"));
        Assert.True(clonedScope.ReferenceExists("id"));
    }

    [Fact(DisplayName = "changing a value in a cloned scope does not change it in the original one")]
    public void changing_a_value_in_a_cloned_scope_does_not_change_it_in_the_original_one()
    {
        var scope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id", new StateElement { Value = 1 } } });
        var clonedScope = scope.Clone();
        clonedScope.Update("id", 2);
        Assert.Equal(1, scope.Get("id"));
        Assert.Equal(2, clonedScope.Get("id"));
    }

    [Fact(DisplayName = "changing a value in a scope does not change it in its clone")]
    public void changing_a_value_in_a_scope_does_not_change_it_in_its_clone()
    {
        var scope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id", new StateElement { Value = 1 } } });
        var clonedScope = scope.Clone();
        scope.Update("id", 2);
        Assert.Equal(2, scope.Get("id"));
        Assert.Equal(1, clonedScope.Get("id"));
    }

    [Fact(DisplayName = "aggregating another scope squashes it into the current one")]
    public void aggregating_another_scope_squashes_it_into_the_current_one()
    {
        var scope1 = new ReferenceScope(new Dictionary<string, StateElement>() { { "id1", new StateElement { Value = 1 } } });
        var scope2 = new ReferenceScope(new Dictionary<string, StateElement>() { { "id2", new StateElement { Value = 2 } } });
        var scope = scope1.Aggregate(scope2);
        Assert.Equal(1, scope.Get("id1"));
        Assert.Equal(2, scope.Get("id2"));
    }

    [Fact(DisplayName = "when aggregating two scopes that have common references priority is given to the other scope")]
    public void when_aggregating_two_scopes_that_have_common_references_priority_is_given_to_the_other_scope()
    {
        var scope1 = new ReferenceScope(new Dictionary<string, StateElement>() { { "id", new StateElement { Value = 1 } } });
        var scope2 = new ReferenceScope(new Dictionary<string, StateElement>() { { "id", new StateElement { Value = 2 } } });
        var scope = scope1.Aggregate(scope2);
        Assert.Equal(2, scope.Get("id"));
    }
}