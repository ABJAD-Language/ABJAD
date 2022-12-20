using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Expressions.Strategies;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Expressions.Primitives;
using ABJAD.InterpretEngine.Types;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Expressions.Strategies;

public class PrimitiveEvaluationStrategyTest
{
    private readonly ScopeFacade scopeFacade;

    public PrimitiveEvaluationStrategyTest()
    {
        scopeFacade = Substitute.For<ScopeFacade>();
    }
    
    [Fact(DisplayName = "returns the value of the bool when the primitive is bool")]
    public void returns_the_value_of_the_bool_when_the_primitive_is_bool()
    {
        var strategy = new PrimitiveEvaluationStrategy(new BoolPrimitive { Value = true }, scopeFacade);
        var result = strategy.Apply();
        Assert.True(result.Type.IsBool());
        Assert.True((bool)result.Value);
    }

    [Fact(DisplayName = "returns the value of the number when the primitive is number")]
    public void returns_the_value_of_the_number_when_the_primitive_is_number()
    {
        var strategy = new PrimitiveEvaluationStrategy(new NumberPrimitive { Value = 3.0 }, scopeFacade);
        var result = strategy.Apply();
        Assert.True(result.Type.IsNumber());
        Assert.Equal(3.0, result.Value);
    }

    [Fact(DisplayName = "returns the value of the string when the primitive is string")]
    public void returns_the_value_of_the_string_when_the_primitive_is_string()
    {
        var value = Guid.NewGuid().ToString();
        var strategy = new PrimitiveEvaluationStrategy(new StringPrimitive() { Value = value }, scopeFacade);
        var result = strategy.Apply();
        Assert.True(result.Type.IsString());
        Assert.Equal(value, result.Value);
    }

    [Fact(DisplayName = "returns null when the primitive is null")]
    public void returns_null_when_the_primitive_is_null()
    {
        var strategy = new PrimitiveEvaluationStrategy(new NullPrimitive(), scopeFacade);
        var result = strategy.Apply();
        Assert.True(result.Type.IsUndefined());
        Assert.Equal(SpecialValues.NULL, result.Value);
    }

    [Fact(DisplayName = "returns the value of the identifier when the primitive is identifier")]
    public void returns_the_value_of_the_identifier_when_the_primitive_is_identifier()
    {
        scopeFacade.ReferenceExists("id").Returns(true);
        scopeFacade.Get("id").Returns(2);
        scopeFacade.GetType("id").Returns(DataType.Number());
        var strategy = new PrimitiveEvaluationStrategy(new IdentifierPrimitive { Value = "id" }, scopeFacade);
        var result = strategy.Apply();
        Assert.True(result.Type.IsNumber());
        Assert.Equal(2, result.Value);
    }

    [Fact(DisplayName = "throw error if failed to retrieve the value of an identifier")]
    public void throw_error_if_failed_to_retrieve_the_value_of_an_identifier()
    {
        scopeFacade.ReferenceExists("id").Returns(false);
        var strategy = new PrimitiveEvaluationStrategy(new IdentifierPrimitive { Value = "id" }, scopeFacade);
        Assert.Throws<ReferenceNameDoesNotExistException>(() => strategy.Apply());
    }
}