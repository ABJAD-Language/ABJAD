using ABJAD.InterpretEngine.Expressions.Strategies;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Declarations;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Types;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Expressions.Strategies;

public class InstantiationEvaluationStrategyTest
{
    private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
    private readonly ScopeFacade globalScope = Substitute.For<ScopeFacade>();
    private readonly ScopeFacade localScope = Substitute.For<ScopeFacade>();
    private readonly Interpreter<Statement> statementInterpreter = Substitute.For<Interpreter<Statement>>();
    private readonly Interpreter<Declaration> declarationInterpreter = Substitute.For<Interpreter<Declaration>>();

    [Fact(DisplayName = "throws error if class does not exist")]
    public void throws_error_if_class_does_not_exist()
    {
        var instantiation = new Instantiation() { ClassName = "class" };
        globalScope.TypeExists("class").Returns(false);
        var strategy = new InstantiationEvaluationStrategy(instantiation, globalScope, localScope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        Assert.Throws<ClassNotFoundException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error if any of the arguments evaluates to undefined")]
    public void throws_error_if_any_of_the_arguments_evaluates_to_undefined()
    {
        var argument = Substitute.For<Expression>();
        var instantiation = new Instantiation() { ClassName = "class", Arguments = new List<Expression> { argument }};
        globalScope.TypeExists("class").Returns(true);
        expressionEvaluator.Evaluate(argument).Returns(new EvaluatedResult { Value = SpecialValues.UNDEFINED });
        var strategy = new InstantiationEvaluationStrategy(instantiation, globalScope, localScope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        Assert.Throws<OperationOnUndefinedValueException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error if the target class does not have a matching non parameterless constructor")]
    public void throws_error_if_the_target_class_does_not_have_a_matching_non_parameterless_constructor()
    {
        var argument = Substitute.For<Expression>();
        var instantiation = new Instantiation() { ClassName = "class", Arguments = new List<Expression> { argument }};
        expressionEvaluator.Evaluate(argument).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = true });
        
        globalScope.TypeExists("class").Returns(true);
        globalScope.TypeHasConstructor("class", DataType.Bool()).Returns(false);
        var strategy = new InstantiationEvaluationStrategy(instantiation, globalScope, localScope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        Assert.Throws<NoSuitableConstructorFoundException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "applies a default constructor when the target has not defined a parameterless one")]
    public void applies_a_default_constructor_when_the_target_has_not_defined_a_parameterless_one()
    {
        var instantiation = new Instantiation() { ClassName = "class", Arguments = new List<Expression>() };
        
        globalScope.TypeExists("class").Returns(true);
        globalScope.TypeHasConstructor("class").Returns(false);
        globalScope.GetType("class").Returns(new ClassElement() { Declarations = new List<Declaration>() });
        var strategy = new InstantiationEvaluationStrategy(instantiation, globalScope, localScope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        var exception = Record.Exception(() => strategy.Apply());
        Assert.Null(exception);
    }

    [Fact(DisplayName = "interprets the class body declarations on the happy path")]
    public void interprets_the_class_body_declarations_on_the_happy_path()
    {
        var instantiation = new Instantiation() { ClassName = "class", Arguments = new List<Expression>() };
        
        globalScope.TypeExists("class").Returns(true);
        var declaration = Substitute.For<Declaration>();
        globalScope.GetType("class").Returns(new ClassElement() { Declarations = new List<Declaration> { declaration } });
        globalScope.GetTypeConstructor("class").Returns(new ConstructorElement { Body = new Block() });
        var strategy = new InstantiationEvaluationStrategy(instantiation, globalScope, localScope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        strategy.Apply();
        
        declarationInterpreter.Received(1).Interpret(declaration);
    }

    [Fact(DisplayName = "interprets the constructor body on the happy path if exists")]
    public void interprets_the_constructor_body_on_the_happy_path_if_exists()
    {
        var arg = Substitute.For<Expression>();
        var argType = Substitute.For<DataType>();
        var argValue = new object();
        expressionEvaluator.Evaluate(arg).Returns(new EvaluatedResult { Type = argType, Value = argValue });
        var instantiation = new Instantiation() { ClassName = "class", Arguments = new List<Expression>() { arg } };
        
        globalScope.TypeExists("class").Returns(true);
        globalScope.GetType("class").Returns(new ClassElement() { Declarations = new List<Declaration>() });
        globalScope.TypeHasConstructor("class", argType).Returns(true);
        var constructorBody = new Block();
        var constructorElement = new ConstructorElement()
        {
            Body = constructorBody, 
            Parameters = new List<FunctionParameter>() { new() { Type = argType, Name = "param" }}
        };
        globalScope.GetTypeConstructor("class", argType).Returns(constructorElement);
        var strategy = new InstantiationEvaluationStrategy(instantiation, globalScope, localScope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        strategy.Apply();
        
        Received.InOrder(() =>
        {
            globalScope.AddNewScope();
            globalScope.DefineVariable("param", argType, argValue);
        });
        statementInterpreter.Received(1).Interpret(constructorBody);
    }

    [Fact(DisplayName = "returns an instance element with the local scope included")]
    public void returns_an_instance_element_with_the_local_scope_included()
    {
        var instantiation = new Instantiation() { ClassName = "class", Arguments = new List<Expression>() };
        globalScope.TypeExists("class").Returns(true);
        globalScope.GetType("class").Returns(new ClassElement() { Declarations = new List<Declaration>() });
        var strategy = new InstantiationEvaluationStrategy(instantiation, globalScope, localScope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        var result = strategy.Apply();
        Assert.True(result.Type.Is("class"));
        Assert.Equal(localScope, (result.Value as InstanceElement).Scope);
    }
}