using ABJAD.Interpreter.Domain;
using ABJAD.Interpreter.Domain.Declarations;
using ABJAD.Interpreter.Domain.Expressions;
using ABJAD.Interpreter.Domain.Expressions.Strategies;
using ABJAD.Interpreter.Domain.ScopeManagement;
using ABJAD.Interpreter.Domain.Shared;
using ABJAD.Interpreter.Domain.Shared.Declarations;
using ABJAD.Interpreter.Domain.Shared.Expressions;
using ABJAD.Interpreter.Domain.Shared.Statements;
using ABJAD.Interpreter.Domain.Statements;
using ABJAD.Interpreter.Domain.Types;
using NSubstitute;

namespace ABJAD.Test.Interpreter.Domain.Expressions.Strategies;

public class MethodCallEvaluationStrategyTest
{
    private readonly ScopeFacade scope = Substitute.For<ScopeFacade>();
    private readonly IExpressionEvaluator expressionEvaluator = Substitute.For<IExpressionEvaluator>();
    private readonly IStatementInterpreter statementInterpreter = Substitute.For<IStatementInterpreter>();
    private readonly IDeclarationInterpreter declarationInterpreter = Substitute.For<IDeclarationInterpreter>();

    [Fact(DisplayName = "throws error when method does not exist")]
    public void throws_error_when_method_does_not_exist()
    {
        var arg = Substitute.For<Expression>();

        var argType = Substitute.For<DataType>();
        expressionEvaluator.Evaluate(arg).Returns(new EvaluatedResult { Type = argType, Value = new object() });

        scope.FunctionExists("method", argType).Returns(false);

        var methodCall = new MethodCall() { MethodName = "method", Arguments = new List<Expression> { arg } };
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

        var methodCall = new MethodCall() { MethodName = "method", Arguments = new List<Expression> { arg } };
        var strategy = new MethodCallEvaluationStrategy(methodCall, scope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        Assert.Throws<OperationOnUndefinedValueException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "assigns the arguments to the parameter names in the scope")]
    public void assigns_the_arguments_to_the_parameter_names_in_the_scope()
    {
        var arg = Substitute.For<Expression>();

        var argType = Substitute.For<DataType>();
        var argValue = new object();
        expressionEvaluator.Evaluate(arg).Returns(new EvaluatedResult { Type = argType, Value = argValue });

        scope.FunctionExists("method", argType).Returns(true);
        scope.GetFunction("method", argType).Returns(new FunctionElement()
        {
            Parameters = new List<FunctionParameter> { new() { Type = argType, Name = "param" } },
            Body = new Block() { Bindings = new List<Binding>() }
        });

        var methodCall = new MethodCall() { MethodName = "method", Arguments = new List<Expression> { arg } };
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
        statementInterpreter.Interpret(statement, true).Returns(StatementInterpretationResult.GetNotReturned());
        strategy.Apply();

        Received.InOrder(() =>
        {
            declarationInterpreter.Interpret(declaration);
            statementInterpreter.Interpret(statement, true);
        });
    }

    [Fact(DisplayName = "does not continue with the interpretation if a returning value was returned")]
    public void does_not_continue_with_the_interpretation_if_a_returning_value_was_returned()
    {
        var declaration1 = Substitute.For<Declaration>();
        var declaration2 = Substitute.For<Declaration>();
        var statement1 = Substitute.For<Statement>();
        var statement2 = Substitute.For<Statement>();
        var functionBody = new Block() { Bindings = new List<Binding> { declaration1, statement1, statement2, declaration2 } };
        var functionElement = new FunctionElement
        {
            Parameters = new List<FunctionParameter>(),
            Body = functionBody
        };
        scope.FunctionExists("method").Returns(true);
        scope.GetFunction("method").Returns(functionElement);

        statementInterpreter.Interpret(statement1, true).Returns(StatementInterpretationResult.GetReturned());

        var methodCall = new MethodCall { MethodName = "method", Arguments = new List<Expression>() };
        var strategy = new MethodCallEvaluationStrategy(methodCall, scope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        strategy.Apply();

        declarationInterpreter.Received(1).Interpret(declaration1);
        statementInterpreter.Received(1).Interpret(statement1, true);
        statementInterpreter.DidNotReceive().Interpret(statement2, true);
        declarationInterpreter.DidNotReceive().Interpret(declaration2);
    }

    [Fact(DisplayName = "removes the defined parameter values from the scope after finishing")]
    public void removes_the_defined_parameter_values_from_the_scope_after_finishing()
    {
        var arg = Substitute.For<Expression>();

        var argType = Substitute.For<DataType>();
        var argValue = new object();
        expressionEvaluator.Evaluate(arg).Returns(new EvaluatedResult { Type = argType, Value = argValue });

        scope.FunctionExists("method", argType).Returns(true);
        var declaration = Substitute.For<Declaration>();
        scope.GetFunction("method", argType).Returns(new FunctionElement()
        {
            Parameters = new List<FunctionParameter> { new() { Type = argType, Name = "param" } },
            Body = new Block() { Bindings = new List<Binding>() { declaration } }
        });

        var methodCall = new MethodCall() { MethodName = "method", Arguments = new List<Expression> { arg } };
        var strategy = new MethodCallEvaluationStrategy(methodCall, scope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        strategy.Apply();

        Received.InOrder(() =>
        {
            declarationInterpreter.Interpret(declaration);
            scope.RemoveLastScope();
        });
    }

    [Fact(DisplayName = "throws error if the method was supposed to return a value but no statement returned a returning result")]
    public void throws_error_if_the_method_was_supposed_to_return_a_value_but_no_statement_returned_a_returning_result()
    {
        var statement = Substitute.For<Statement>();
        var functionBody = new Block() { Bindings = new List<Binding> { statement } };
        var functionElement = new FunctionElement
        {
            Parameters = new List<FunctionParameter>(),
            ReturnType = Substitute.For<DataType>(),
            Body = functionBody
        };
        scope.FunctionExists("method").Returns(true);
        scope.GetFunction("method").Returns(functionElement);

        statementInterpreter.Interpret(statement, true).Returns(StatementInterpretationResult.GetNotReturned());

        var methodCall = new MethodCall { MethodName = "method", Arguments = new List<Expression>() };
        var strategy = new MethodCallEvaluationStrategy(methodCall, scope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        Assert.Throws<NoValueReturnedException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws an error if the method was not supposed to return a value but did anyway")]
    public void throws_an_error_if_the_method_was_not_supposed_to_return_a_value_but_did_anyway()
    {
        var statement = Substitute.For<Statement>();
        var functionBody = new Block() { Bindings = new List<Binding> { statement } };
        var functionElement = new FunctionElement
        {
            Parameters = new List<FunctionParameter>(),
            Body = functionBody
        };
        scope.FunctionExists("method").Returns(true);
        scope.GetFunction("method").Returns(functionElement);

        statementInterpreter.Interpret(statement, true).Returns(StatementInterpretationResult.GetReturned(new EvaluatedResult()));

        var methodCall = new MethodCall { MethodName = "method", Arguments = new List<Expression>() };
        var strategy = new MethodCallEvaluationStrategy(methodCall, scope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        Assert.Throws<InvalidVoidReturnStatementException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "does throw an error if the method is not supposed to return a value but a statement returned a returning result with no value")]
    public void does_throw_an_error_if_the_method_is_not_supposed_to_return_a_value_but_a_statement_returned_a_returning_result_with_no_value()
    {
        var statement = Substitute.For<Statement>();
        var functionBody = new Block() { Bindings = new List<Binding> { statement } };
        var functionElement = new FunctionElement
        {
            Parameters = new List<FunctionParameter>(),
            Body = functionBody
        };
        scope.FunctionExists("method").Returns(true);
        scope.GetFunction("method").Returns(functionElement);

        statementInterpreter.Interpret(statement, true).Returns(StatementInterpretationResult.GetReturned());

        var methodCall = new MethodCall { MethodName = "method", Arguments = new List<Expression>() };
        var strategy = new MethodCallEvaluationStrategy(methodCall, scope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        strategy.Apply();
    }

    [Fact(DisplayName = "throws error if the returned value is of a different type than the expected one to be returned by the function")]
    public void throws_error_if_the_returned_value_is_of_a_different_type_than_the_expected_one_to_be_returned_by_the_function()
    {
        var statement = Substitute.For<Statement>();
        var functionBody = new Block() { Bindings = new List<Binding> { statement } };
        var returnType = Substitute.For<DataType>();
        var functionElement = new FunctionElement
        {
            Parameters = new List<FunctionParameter>(),
            Body = functionBody,
            ReturnType = returnType
        };
        scope.FunctionExists("method").Returns(true);
        scope.GetFunction("method").Returns(functionElement);

        var returnedValueType = Substitute.For<DataType>();
        var evaluatedResult = new EvaluatedResult() { Type = returnedValueType };
        statementInterpreter.Interpret(statement, true).Returns(StatementInterpretationResult.GetReturned(evaluatedResult));

        var methodCall = new MethodCall { MethodName = "method", Arguments = new List<Expression>() };
        var strategy = new MethodCallEvaluationStrategy(methodCall, scope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        Assert.Throws<InvalidTypeException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "returns the value of the returning result when it appears and matching the function return type")]
    public void returns_the_value_of_the_returning_result_when_it_appears_and_matching_the_function_return_type()
    {
        var statement = Substitute.For<Statement>();
        var functionBody = new Block() { Bindings = new List<Binding> { statement } };
        var returnType = Substitute.For<DataType>();
        var functionElement = new FunctionElement
        {
            Parameters = new List<FunctionParameter>(),
            Body = functionBody,
            ReturnType = returnType
        };
        scope.FunctionExists("method").Returns(true);
        scope.GetFunction("method").Returns(functionElement);

        var returnedValueType = Substitute.For<DataType>();
        var evaluatedResult = new EvaluatedResult() { Type = returnedValueType };
        statementInterpreter.Interpret(statement, true).Returns(StatementInterpretationResult.GetReturned(evaluatedResult));

        returnType.Is(returnedValueType).Returns(true);

        var methodCall = new MethodCall { MethodName = "method", Arguments = new List<Expression>() };
        var strategy = new MethodCallEvaluationStrategy(methodCall, scope, expressionEvaluator, statementInterpreter, declarationInterpreter);

        Assert.Equal(evaluatedResult, strategy.Apply());
    }

    [Fact(DisplayName = "returns an undefined type and value when the function does not have a return type")]
    public void returns_an_undefined_type_and_value_when_the_function_does_not_have_a_return_type()
    {
        var statement = Substitute.For<Statement>();
        var functionBody = new Block() { Bindings = new List<Binding> { statement } };
        var functionElement = new FunctionElement
        {
            Parameters = new List<FunctionParameter>(),
            Body = functionBody,
        };
        scope.FunctionExists("method").Returns(true);
        scope.GetFunction("method").Returns(functionElement);

        statementInterpreter.Interpret(statement, true).Returns(StatementInterpretationResult.GetNotReturned());

        var methodCall = new MethodCall { MethodName = "method", Arguments = new List<Expression>() };
        var strategy = new MethodCallEvaluationStrategy(methodCall, scope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        var evaluatedResult = strategy.Apply();
        Assert.True(evaluatedResult.Type.IsUndefined());
        Assert.Equal(SpecialValues.UNDEFINED, evaluatedResult.Value);
    }

    [Fact(DisplayName = "returns a result with special value null when the returned result from the body was so")]
    public void returns_a_result_with_special_value_null_when_the_returned_result_from_the_body_was_so()
    {
        var statement = Substitute.For<Statement>();
        var functionBody = new Block() { Bindings = new List<Binding> { statement } };
        var functionElement = new FunctionElement
        {
            Parameters = new List<FunctionParameter>(),
            Body = functionBody,
            ReturnType = DataType.String()
        };
        scope.FunctionExists("method").Returns(true);
        scope.GetFunction("method").Returns(functionElement);

        var result = new EvaluatedResult() { Type = DataType.Undefined(), Value = SpecialValues.NULL };
        statementInterpreter.Interpret(statement, true).Returns(StatementInterpretationResult.GetReturned(result));

        var methodCall = new MethodCall { MethodName = "method", Arguments = new List<Expression>() };
        var strategy = new MethodCallEvaluationStrategy(methodCall, scope, expressionEvaluator, statementInterpreter, declarationInterpreter);
        var evaluatedResult = strategy.Apply();

        Assert.True(evaluatedResult.Type.IsString());
        Assert.Equal(SpecialValues.NULL, evaluatedResult.Value);
    }
}