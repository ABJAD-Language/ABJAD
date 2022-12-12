using ABJAD.InterpretEngine;
using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Expressions.Strategies;
using ABJAD.InterpretEngine.Shared.Expressions.Primitives;
using NSubstitute;
using Xunit.Sdk;

namespace ABJAD.InterpretEngine.Test.Expressions.Strategies;

public class PrimitiveInterpretingStrategyTest
{
    private readonly IScope scope;

    public PrimitiveInterpretingStrategyTest()
    {
        scope = Substitute.For<IScope>();
    }
    
    [Fact(DisplayName = "returns the value of the bool when the primitive is bool")]
    public void returns_the_value_of_the_bool_when_the_primitive_is_bool()
    {
        var strategy = new PrimitiveInterpretingStrategy(new BoolPrimitive { Value = true }, scope);
        Assert.True(strategy.Apply() is bool);
        Assert.True((bool)strategy.Apply());
    }

    [Fact(DisplayName = "returns the value of the number when the primitive is number")]
    public void returns_the_value_of_the_number_when_the_primitive_is_number()
    {
        var strategy = new PrimitiveInterpretingStrategy(new NumberPrimitive { Value = 3.0 }, scope);
        Assert.True(strategy.Apply() is double);
        Assert.Equal(3.0, strategy.Apply());
    }

    [Fact(DisplayName = "returns the value of the string when the primitive is string")]
    public void returns_the_value_of_the_string_when_the_primitive_is_string()
    {
        var value = Guid.NewGuid().ToString();
        var strategy = new PrimitiveInterpretingStrategy(new StringPrimitive() { Value = value }, scope);
        Assert.True(strategy.Apply() is string);
        Assert.Equal(value, strategy.Apply());
    }

    [Fact(DisplayName = "returns null when the primitive is null")]
    public void returns_null_when_the_primitive_is_null()
    {
        var strategy = new PrimitiveInterpretingStrategy(new NullPrimitive(), scope);
        Assert.Null(strategy.Apply());
    }

    [Fact(DisplayName = "returns the value of the identifier when the primitive is identifier")]
    public void returns_the_value_of_the_identifier_when_the_primitive_is_identifier()
    {
        scope.ReferenceExists("id").Returns(true);
        scope.Get("id").Returns(2);
        var strategy = new PrimitiveInterpretingStrategy(new IdentifierPrimitive { Value = "id" }, scope);
        Assert.Equal(2, strategy.Apply());
    }

    [Fact(DisplayName = "throw error if failed to retrieve the value of an identifier")]
    public void throw_error_if_failed_to_retrieve_the_value_of_an_identifier()
    {
        scope.ReferenceExists("id").Returns(false);
        var strategy = new PrimitiveInterpretingStrategy(new IdentifierPrimitive { Value = "id" }, scope);
        Assert.Throws<ReferenceNameDoesNotExistException>(() => strategy.Apply());
    }
}