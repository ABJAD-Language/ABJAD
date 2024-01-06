using ABJAD.Interpreter.Domain.Expressions;
using ABJAD.Interpreter.Domain.Expressions.Strategies;
using ABJAD.Interpreter.Domain.ScopeManagement;
using ABJAD.Interpreter.Domain.Shared.Expressions;
using ABJAD.Interpreter.Domain.Types;
using NSubstitute;

namespace ABJAD.Test.Interpreter.Domain.Expressions.Strategies;

public class InstanceFieldAccessEvaluationStrategyTest
{
    private readonly ScopeFacade scope = Substitute.For<ScopeFacade>();

    [Fact(DisplayName = "throws error if the list of fields is empty")]
    public void throws_error_if_the_list_of_fields_is_empty()
    {
        var instanceFieldAccess = new InstanceFieldAccess() { Instance = "instance" };
        var strategy = new InstanceFieldAccessEvaluationStrategy(instanceFieldAccess, scope);
        Assert.Throws<ArgumentException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error if the instance reference does not exist")]
    public void throws_error_if_the_instance_reference_does_not_exist()
    {
        scope.ReferenceExists("instance").Returns(false);
        var instanceFieldAccess = new InstanceFieldAccess() { Instance = "instance", NestedFields = new List<string> { "field" } };
        var strategy = new InstanceFieldAccessEvaluationStrategy(instanceFieldAccess, scope);
        Assert.Throws<ReferenceNameDoesNotExistException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error if the reference is not an instance")]
    public void throws_error_if_the_reference_is_not_an_instance()
    {
        scope.ReferenceExists("instance").Returns(true);
        scope.GetReference("instance").Returns(new object());
        var instanceFieldAccess = new InstanceFieldAccess() { Instance = "instance", NestedFields = new List<string> { "field" } };
        var strategy = new InstanceFieldAccessEvaluationStrategy(instanceFieldAccess, scope);
        Assert.Throws<NotInstanceReferenceException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error if any of the nested fields does not exist")]
    public void throws_error_if_any_of_the_nested_fields_does_not_exist()
    {
        scope.ReferenceExists("instance").Returns(true);
        var instanceFieldAccess = new InstanceFieldAccess() { Instance = "instance", NestedFields = new List<string> { "field1", "field2" } };
        var innerScope = Substitute.For<ScopeFacade>();
        scope.GetReference("instance").Returns(new InstanceElement() { Scope = innerScope });
        innerScope.ReferenceExists("field1").Returns(true);
        var innerScope1 = Substitute.For<ScopeFacade>();
        innerScope.GetReference("field1").Returns(new InstanceElement() { Scope = innerScope1 });
        innerScope1.ReferenceExists("field2").Returns(false);

        var strategy = new InstanceFieldAccessEvaluationStrategy(instanceFieldAccess, scope);
        Assert.Throws<ReferenceNameDoesNotExistException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error if any of the nested fields is not an instance and references a field")]
    public void throws_error_if_any_of_the_nested_fields_is_not_an_instance_and_references_a_field()
    {
        scope.ReferenceExists("instance").Returns(true);
        var instanceFieldAccess = new InstanceFieldAccess() { Instance = "instance", NestedFields = new List<string> { "field1", "field2" } };
        var innerScope = Substitute.For<ScopeFacade>();
        scope.GetReference("instance").Returns(new InstanceElement() { Scope = innerScope });
        innerScope.ReferenceExists("field1").Returns(true);
        innerScope.GetReference("field1").Returns(new object());

        var strategy = new InstanceFieldAccessEvaluationStrategy(instanceFieldAccess, scope);
        Assert.Throws<NotInstanceReferenceException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "returns the value of the inner field on the happy path")]
    public void returns_the_value_of_the_inner_field_on_the_happy_path()
    {
        scope.ReferenceExists("instance").Returns(true);
        var instanceFieldAccess = new InstanceFieldAccess() { Instance = "instance", NestedFields = new List<string> { "field1", "field2" } };

        var innerScope = Substitute.For<ScopeFacade>();
        scope.GetReference("instance").Returns(new InstanceElement() { Scope = innerScope });
        innerScope.ReferenceExists("field1").Returns(true);

        var innerScope1 = Substitute.For<ScopeFacade>();
        innerScope.GetReference("field1").Returns(new InstanceElement() { Scope = innerScope1 });
        innerScope1.ReferenceExists("field2").Returns(true);

        var fieldType = Substitute.For<DataType>();
        var fieldValue = new object();
        innerScope1.GetReferenceType("field2").Returns(fieldType);
        innerScope1.GetReference("field2").Returns(fieldValue);

        var strategy = new InstanceFieldAccessEvaluationStrategy(instanceFieldAccess, scope);
        var result = strategy.Apply();
        Assert.Equal(fieldType, result.Type);
        Assert.Equal(fieldValue, result.Value);
    }
}