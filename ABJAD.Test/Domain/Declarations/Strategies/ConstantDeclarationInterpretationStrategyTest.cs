using ABJAD.Interpreter.Domain.Declarations;
using ABJAD.Interpreter.Domain.Declarations.Strategies;
using ABJAD.Interpreter.Domain.ScopeManagement;
using ABJAD.Interpreter.Domain.Shared.Declarations;
using ABJAD.Interpreter.Domain.Shared.Expressions.Primitives;
using ABJAD.Interpreter.Domain.Types;
using NSubstitute;

namespace ABJAD.Test.Domain.Declarations.Strategies;

public class ConstantDeclarationInterpretationStrategyTest
{
    private readonly ScopeFacade scope = Substitute.For<ScopeFacade>();

    [Fact(DisplayName = "throws error if the reference already existed in the current scope")]
    public void throws_error_if_the_reference_already_existed_in_the_current_scope()
    {
        scope.ReferenceExistsInCurrentScope("id").Returns(true);
        var constantDeclaration = new ConstantDeclaration() { Name = "id" };
        var strategy = new ConstantDeclarationInterpretationStrategy(constantDeclaration, scope);
        Assert.Throws<ReferenceAlreadyExistsException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error if the constant type was number but value was not a number primitive")]
    public void throws_error_if_the_constant_type_was_number_but_value_was_not_a_number_primitive()
    {
        var constantDeclaration = new ConstantDeclaration() { Type = DataType.Number(), Value = Substitute.For<Primitive>() };
        var strategy = new ConstantDeclarationInterpretationStrategy(constantDeclaration, scope);
        Assert.Throws<ConstantDeclarationFailureException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "updates scope with the new reference when it is a number")]
    public void updates_scope_with_the_new_reference_when_it_is_a_number()
    {
        var constantDeclaration = new ConstantDeclaration() { Type = DataType.Number(), Value = new NumberPrimitive { Value = 13.0 }, Name = "id" };
        var strategy = new ConstantDeclarationInterpretationStrategy(constantDeclaration, scope);
        strategy.Apply();
        scope.Received(1).DefineConstant("id", DataType.Number(), 13.0);
    }

    [Fact(DisplayName = "throws error if the constant type is string and value is not a string primitive")]
    public void throws_error_if_the_constant_type_is_string_and_value_is_not_a_string_primitive()
    {
        var constantDeclaration = new ConstantDeclaration() { Type = DataType.String(), Value = Substitute.For<Primitive>() };
        var strategy = new ConstantDeclarationInterpretationStrategy(constantDeclaration, scope);
        Assert.Throws<ConstantDeclarationFailureException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "does not throw an error if the constant type is string and value is null")]
    public void does_not_throw_an_error_if_the_constant_type_is_string_and_value_is_null()
    {
        var constantDeclaration = new ConstantDeclaration() { Type = DataType.String(), Value = new NullPrimitive() };
        var strategy = new ConstantDeclarationInterpretationStrategy(constantDeclaration, scope);
        strategy.Apply();
    }

    [Fact(DisplayName = "updates scope with the new reference when it is a string")]
    public void updates_scope_with_the_new_reference_when_it_is_a_string()
    {
        var constantDeclaration = new ConstantDeclaration() { Type = DataType.String(), Value = new StringPrimitive() { Value = "hello" }, Name = "id" };
        var strategy = new ConstantDeclarationInterpretationStrategy(constantDeclaration, scope);
        strategy.Apply();
        scope.Received(1).DefineConstant("id", DataType.String(), "hello");
    }

    [Fact(DisplayName = "throws error if the constant type is bool and value is not a bool primitive")]
    public void throws_error_if_the_constant_type_is_bool_and_value_is_not_a_bool_primitive()
    {
        var constantDeclaration = new ConstantDeclaration() { Type = DataType.Bool(), Value = Substitute.For<Primitive>() };
        var strategy = new ConstantDeclarationInterpretationStrategy(constantDeclaration, scope);
        Assert.Throws<ConstantDeclarationFailureException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "updates scope with the new reference when it is a bool")]
    public void updates_scope_with_the_new_reference_when_it_is_a_bool()
    {
        var constantDeclaration = new ConstantDeclaration() { Type = DataType.Bool(), Value = new BoolPrimitive() { Value = true }, Name = "id" };
        var strategy = new ConstantDeclarationInterpretationStrategy(constantDeclaration, scope);
        strategy.Apply();
        scope.Received(1).DefineConstant("id", DataType.Bool(), true);
    }

    [Fact(DisplayName = "throws error if the constant type is neither of the accepted ones")]
    public void throws_error_if_the_constant_type_is_neither_of_the_accepted_ones()
    {
        var constantType = Substitute.For<DataType>();
        var constantDeclaration = new ConstantDeclaration() { Type = constantType };
        constantType.IsBool().Returns(false);
        constantType.IsNumber().Returns(false);
        constantType.IsString().Returns(false);
        var strategy = new ConstantDeclarationInterpretationStrategy(constantDeclaration, scope);
        Assert.Throws<ConstantDeclarationFailureException>(() => strategy.Apply());
    }
}