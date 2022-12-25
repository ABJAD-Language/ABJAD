using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Expressions.Strategies;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared;
using ABJAD.InterpretEngine.Shared.Declarations;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Types;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Expressions.Strategies;

public class MethodCallEvaluationStrategyTest
{
    private readonly ScopeFacade scope = Substitute.For<ScopeFacade>();
    private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
    private readonly Interpreter<Statement> statementInterpreter = Substitute.For<Interpreter<Statement>>();
    private readonly Interpreter<Declaration> declarationInterpreter = Substitute.For<Interpreter<Declaration>>();

    [Fact(DisplayName = "throws error when method does not exist")]
    public void throws_error_when_method_does_not_exist()
    {
        var arg = Substitute.For<Expression>();

        var argType = Substitute.For<DataType>();
        expressionEvaluator.Evaluate(arg).Returns(new EvaluatedResult { Type = argType, Value = new object() });

        scope.FunctionExists("method", argType).Returns(false);
        
        var methodCall = new MethodCall() { MethodName = "method", Arguments = new List<Expression> { arg }};
        var strategy = new MethodCallEvaluationStrategy(methodCall, scope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        Assert.Throws<MethodNotFoundException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error when the arguments evaluate to undefined")]
    public void throws_error_when_the_arguments_evaluate_to_undefined()
    {
        var arg = Substitute.For<Expression>();

        var argType = Substitute.For<DataType>();
        expressionEvaluator.Evaluate(arg).Returns(new EvaluatedResult { Type = argType, Value = SpecialValues.UNDEFINED });

        scope.FunctionExists("method", argType).Returns(true);
        scope.GetFunction("method", argType).Returns(new FunctionElement()
        {
            Parameters = new List<FunctionParameter> { new() { Type = argType, Name = "param" } },
            Body = new Block() { Bindings = new List<Binding>() }
        });
        
        var methodCall = new MethodCall() { MethodName = "method", Arguments = new List<Expression> { arg }};
        var strategy = new MethodCallEvaluationStrategy(methodCall, scope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        Assert.Throws<OperationOnUndefinedValueException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "assigns the arguments to the parameter names in the scope")]
    public void assigns_the_arguments_to_the_parameter_names_in_the_scope()
    {
        var arg = Substitute.For<Expression>();

        var argType = Substitute.For<DataType>();
        var argValue = new object();
        expressionEvaluator.Evaluate(arg).Returns(new EvaluatedResult { Type = argType, Value = argValue});

        scope.FunctionExists("method", argType).Returns(true);
        scope.GetFunction("method", argType).Returns(new FunctionElement()
        {
            Parameters = new List<FunctionParameter> { new() { Type = argType, Name = "param" } },
            Body = new Block() { Bindings = new List<Binding>() }
        });
        
        var methodCall = new MethodCall() { MethodName = "method", Arguments = new List<Expression> { arg }};
        var strategy = new MethodCallEvaluationStrategy(methodCall, scope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        strategy.Apply();
        
        Received.InOrder(() =>
        {
            scope.AddNewScope();
            scope.DefineVariable("param", argType, argValue);
        });
    }

    [Fact(DisplayName = "interprets the bindings of the method body on the happy path")]
    public void interprets_the_bindings_of_the_method_body_on_the_happy_path()
    {
        var declaration = Substitute.For<Declaration>();
        var statement = Substitute.For<Statement>();
        var functionBody = new Block() { Bindings = new List<Binding> { declaration, statement } };
        var functionElement = new FunctionElement
        {
            Parameters = new List<FunctionParameter>(),
            Body = functionBody
        };
        scope.FunctionExists("method").Returns(true);
        scope.GetFunction("method").Returns(functionElement);
        
        var methodCall = new MethodCall { MethodName = "method", Arguments = new List<Expression>() };
        var strategy = new MethodCallEvaluationStrategy(methodCall, scope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        strategy.Apply();
        
        Received.InOrder(() =>
        {
            declarationInterpreter.Interpret(declaration);
            statementInterpreter.Interpret(statement);
        });
    }

    [Fact(DisplayName = "does not call the statement interpreter on a return statement")]
    public void does_not_call_the_statement_interpreter_on_a_return_statement()
    {
        var returnTarget = Substitute.For<Expression>();
        expressionEvaluator.Evaluate(returnTarget).Returns(new EvaluatedResult() { Type = DataType.String() });
        var returnStatement = new Return() { Target = returnTarget};
        var functionBody = new Block() { Bindings = new List<Binding> { returnStatement } };
        var functionElement = new FunctionElement
        {
            Parameters = new List<FunctionParameter>(),
            Body = functionBody,
            ReturnType = DataType.String()
        };
        scope.FunctionExists("method").Returns(true);
        scope.GetFunction("method").Returns(functionElement);
        
        var methodCall = new MethodCall { MethodName = "method", Arguments = new List<Expression>() };
        var strategy = new MethodCallEvaluationStrategy(methodCall, scope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        strategy.Apply();
        
        statementInterpreter.DidNotReceive().Interpret(returnStatement);
    }

    [Fact(DisplayName = "throws error if the expression to return evaluates to a different type from the function return type")]
    public void throws_error_if_the_expression_to_return_evaluates_to_a_different_type_from_the_function_return_type()
    {
        var returnTarget = Substitute.For<Expression>();
        var returnTargetType = Substitute.For<DataType>();
        expressionEvaluator.Evaluate(returnTarget).Returns(new EvaluatedResult() { Type = returnTargetType });
        var returnStatement = new Return() { Target = returnTarget};
        var functionBody = new Block() { Bindings = new List<Binding> { returnStatement } };
        var returnType = Substitute.For<DataType>();
        var functionElement = new FunctionElement
        {
            Parameters = new List<FunctionParameter>(),
            Body = functionBody,
            ReturnType = returnType
        };
        returnTargetType.Is(returnType).Returns(false);
        scope.FunctionExists("method").Returns(true);
        scope.GetFunction("method").Returns(functionElement);
        
        var methodCall = new MethodCall { MethodName = "method", Arguments = new List<Expression>() };
        var strategy = new MethodCallEvaluationStrategy(methodCall, scope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        Assert.Throws<InvalidTypeException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error if the function does not have a return type but the return statement has a target expression")]
    public void throws_error_if_the_function_does_not_have_a_return_type_but_the_return_statement_has_a_target_expression()
    {
        var returnTarget = Substitute.For<Expression>();
        var returnStatement = new Return() { Target = returnTarget};
        var functionBody = new Block() { Bindings = new List<Binding> { returnStatement } };
        var functionElement = new FunctionElement
        {
            Parameters = new List<FunctionParameter>(),
            Body = functionBody
        };
        scope.FunctionExists("method").Returns(true);
        scope.GetFunction("method").Returns(functionElement);
        
        var methodCall = new MethodCall { MethodName = "method", Arguments = new List<Expression>() };
        var strategy = new MethodCallEvaluationStrategy(methodCall, scope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        Assert.Throws<InvalidVoidReturnStatementException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "returns the evaluation of the return target expression")]
    public void returns_the_evaluation_of_the_return_target_expression()
    {
        var returnTarget = Substitute.For<Expression>();
        var returnTargetType = Substitute.For<DataType>();
        returnTargetType.Is(returnTargetType).Returns(true);
        var returnTargetValue = new object();
        expressionEvaluator.Evaluate(returnTarget).Returns(new EvaluatedResult() { Type = returnTargetType, Value = returnTargetValue});
        var returnStatement = new Return() { Target = returnTarget};
        var functionBody = new Block() { Bindings = new List<Binding> { returnStatement } };
        var functionElement = new FunctionElement
        {
            Parameters = new List<FunctionParameter>(),
            Body = functionBody,
            ReturnType =returnTargetType
        };
        scope.FunctionExists("method").Returns(true);
        scope.GetFunction("method").Returns(functionElement);
        
        var methodCall = new MethodCall { MethodName = "method", Arguments = new List<Expression>() };
        var strategy = new MethodCallEvaluationStrategy(methodCall, scope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        var evaluatedResult = strategy.Apply();
        Assert.Equal(returnTargetType, evaluatedResult.Type);
        Assert.Equal(returnTargetValue, evaluatedResult.Value);
    }

    [Fact(DisplayName = "returns an undefined type and value when the function does not have a return type")]
    public void returns_an_undefined_type_and_value_when_the_function_does_not_have_a_return_type()
    {
        var returnStatement = new Return();
        var functionBody = new Block() { Bindings = new List<Binding> { returnStatement } };
        var functionElement = new FunctionElement
        {
            Parameters = new List<FunctionParameter>(),
            Body = functionBody
        };
        scope.FunctionExists("method").Returns(true);
        scope.GetFunction("method").Returns(functionElement);
        
        var methodCall = new MethodCall { MethodName = "method", Arguments = new List<Expression>() };
        var strategy = new MethodCallEvaluationStrategy(methodCall, scope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        var evaluatedResult = strategy.Apply();
        Assert.True(evaluatedResult.Type.IsUndefined());
        Assert.Equal(SpecialValues.UNDEFINED, evaluatedResult.Value);
    }
}