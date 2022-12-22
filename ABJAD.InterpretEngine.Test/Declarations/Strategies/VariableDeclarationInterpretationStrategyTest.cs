using ABJAD.InterpretEngine.Declarations.Strategies;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Declarations;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Types;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Declarations.Strategies;

public class VariableDeclarationInterpretationStrategyTest
{
    private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
    private readonly ScopeFacade scope = Substitute.For<ScopeFacade>();

    
    [Fact(DisplayName = "throws error if the reference exists in the current scope")]
    public void throws_error_if_the_reference_exists_in_the_current_scope()
    {
        scope.ReferenceExistsInCurrentScope("id").Returns(true);
        var variableDeclaration = new VariableDeclaration() { Name = "id" };
        var strategy = new VariableDeclarationInterpretationStrategy(variableDeclaration, scope, expressionEvaluator);
        Assert.Throws<ReferenceAlreadyExistsException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "updates scope with the new reference when value is not passed")]
    public void updates_scope_with_the_new_reference_when_value_is_not_passed()
    {
        scope.ReferenceExistsInCurrentScope("id").Returns(false);
        var variableDeclaration = new VariableDeclaration() { Name = "id", Type = DataType.Number() };
        var strategy = new VariableDeclarationInterpretationStrategy(variableDeclaration, scope, expressionEvaluator);
        strategy.Apply();
        scope.Received(1).DefineVariable("id", DataType.Number(), SpecialValues.UNDEFINED);
    }

    [Fact(DisplayName = "throws error if the type does not match the type of the evaluated expression")]
    public void throws_error_if_the_type_does_not_match_the_type_of_the_evaluated_expression()
    {
        scope.ReferenceExistsInCurrentScope("id").Returns(false);
        var value = Substitute.For<Expression>();
        var variableDeclaration = new VariableDeclaration() { Name = "id", Type = DataType.Number(), Value = value };
        var strategy = new VariableDeclarationInterpretationStrategy(variableDeclaration, scope, expressionEvaluator);
        expressionEvaluator.Evaluate(value).Returns(new EvaluatedResult { Type = DataType.String() });
        Assert.Throws<IncompatibleTypesException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error if the value of the expression evaluated to undefined")]
    public void throws_error_if_the_value_of_the_expression_evaluated_to_undefined()
    {
        scope.ReferenceExistsInCurrentScope("id").Returns(false);
        var value = Substitute.For<Expression>();
        var variableDeclaration = new VariableDeclaration() { Name = "id", Type = DataType.Number(), Value = value };
        var strategy = new VariableDeclarationInterpretationStrategy(variableDeclaration, scope, expressionEvaluator);
        expressionEvaluator.Evaluate(value).Returns(new EvaluatedResult { Type = DataType.Number(), Value = SpecialValues.UNDEFINED });
        Assert.Throws<OperationOnUndefinedValueException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "updates scope with the new reference when the value is passed on the happy path")]
    public void updates_scope_with_the_new_reference_when_the_value_is_passed_on_the_happy_path()
    {
        scope.ReferenceExistsInCurrentScope("id").Returns(false);
        var value = Substitute.For<Expression>();
        var variableDeclaration = new VariableDeclaration() { Name = "id", Type = DataType.Number(), Value = value };
        var strategy = new VariableDeclarationInterpretationStrategy(variableDeclaration, scope, expressionEvaluator);
        expressionEvaluator.Evaluate(value).Returns(new EvaluatedResult { Type = DataType.Number(), Value = 1 });
        strategy.Apply();
        scope.Received(1).DefineVariable("id", DataType.Number(), 1);
    }
}