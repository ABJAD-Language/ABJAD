using ABJAD.InterpretEngine.Declarations;
using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Shared;
using ABJAD.InterpretEngine.Shared.Declarations;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Statements;
using ABJAD.InterpretEngine.Statements.Strategies;
using ABJAD.InterpretEngine.Types;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Statements.Strategies;

public class ForLoopInterpretationStrategyTest
{
    private readonly IStatementInterpreter statementInterpreter = Substitute.For<IStatementInterpreter>();
    private readonly IDeclarationInterpreter declarationInterpreter = Substitute.For<IDeclarationInterpreter>();
    private readonly IExpressionEvaluator expressionEvaluator = Substitute.For<IExpressionEvaluator>();

    [Fact(DisplayName = "calls the declaration interpreter when target is a variable declaration")]
    public void calls_the_declaration_interpreter_when_target_is_a_variable_declaration()
    {
        var targetDefinition = new VariableDeclaration();
        var forLoop = new ForLoop() { TargetDefinition = targetDefinition, Condition = new ExpressionStatement() };
        var strategy = new ForLoopInterpretationStrategy(forLoop, false, statementInterpreter, declarationInterpreter, expressionEvaluator);
        expressionEvaluator.Evaluate(Arg.Any<Expression>()).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = false});
        strategy.Apply();
        declarationInterpreter.Received(1).Interpret(targetDefinition);
    }

    [Fact(DisplayName = "calls the statement interpreter when target is an assignment statement")]
    public void calls_the_statement_interpreter_when_target_is_an_assignment_statement()
    {
        var targetDefinition = new Assignment();
        var forLoop = new ForLoop() { TargetDefinition = targetDefinition, Condition = new ExpressionStatement()};
        var strategy = new ForLoopInterpretationStrategy(forLoop, false, statementInterpreter, declarationInterpreter, expressionEvaluator);
        expressionEvaluator.Evaluate(Arg.Any<Expression>()).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = false });
        strategy.Apply();
        statementInterpreter.Received(1).Interpret(targetDefinition);
    }

    [Fact(DisplayName = "throws error if target definition is neither variable declaration nor assignment statement")]
    public void throws_error_if_target_definition_is_neither_variable_declaration_nor_assignment_statement()
    {
        var forLoop = new ForLoop() { TargetDefinition = Substitute.For<Binding>() };
        var strategy = new ForLoopInterpretationStrategy(forLoop, false, statementInterpreter, declarationInterpreter, expressionEvaluator);
        Assert.Throws<ForLoopInvalidTargetDefinitionException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error when the condition does not evaluate to a bool")]
    public void throws_error_when_the_condition_does_not_evaluate_to_a_bool()
    {
        var condition = Substitute.For<Expression>();
        var forLoop = new ForLoop { TargetDefinition = new Assignment(), Condition = new ExpressionStatement() { Target = condition } };
        var conditionType = Substitute.For<DataType>();
        expressionEvaluator.Evaluate(condition).Returns(new EvaluatedResult { Type = conditionType });
        conditionType.IsBool().Returns(false);
        var strategy = new ForLoopInterpretationStrategy(forLoop, false, statementInterpreter, declarationInterpreter, expressionEvaluator);
        Assert.Throws<InvalidTypeException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "does not execute the body when condition evaluates to false")]
    public void does_not_execute_the_body_when_condition_evaluates_to_false()
    {
        var condition = Substitute.For<Expression>();
        var body = Substitute.For<Statement>();
        var callback = Substitute.For<Expression>();
        var forLoop = new ForLoop
        {
            TargetDefinition = new Assignment(), 
            Condition = new ExpressionStatement() { Target = condition },
            Body = body, Callback = callback
        };
        expressionEvaluator.Evaluate(condition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = false });
        var strategy = new ForLoopInterpretationStrategy(forLoop, false, statementInterpreter, declarationInterpreter, expressionEvaluator);
        strategy.Apply();
        statementInterpreter.DidNotReceive().Interpret(body);
        expressionEvaluator.DidNotReceive().Evaluate(callback);
    }

    [Fact(DisplayName = "executes the body and the callback when the condition evaluates to true")]
    public void executes_the_body_and_the_callback_when_the_condition_evaluates_to_true()
    {
        var condition = Substitute.For<Expression>();
        var body = Substitute.For<Statement>();
        var callback = Substitute.For<Expression>();
        var forLoop = new ForLoop
        {
            TargetDefinition = new Assignment(), 
            Condition = new ExpressionStatement() { Target = condition },
            Body = body, Callback = callback
        };
        expressionEvaluator.Evaluate(condition)
            .Returns(new EvaluatedResult { Type = DataType.Bool(), Value = true },
                new EvaluatedResult { Type = DataType.Bool(), Value = true },
                new EvaluatedResult { Type = DataType.Bool(), Value = false });
        var strategy = new ForLoopInterpretationStrategy(forLoop, false, statementInterpreter, declarationInterpreter, expressionEvaluator);
        strategy.Apply();
        
        Received.InOrder(() =>
        {
            statementInterpreter.Interpret(body);
            expressionEvaluator.Evaluate(callback);
            statementInterpreter.Interpret(body);
            expressionEvaluator.Evaluate(callback);
        });
    }

    [Fact(DisplayName = "executes the body with function context set to true when it is the case")]
    public void executes_the_body_with_function_context_set_to_true_when_it_is_the_case()
    {
        var condition = Substitute.For<Expression>();
        var body = Substitute.For<Statement>();
        var callback = Substitute.For<Expression>();
        var forLoop = new ForLoop
        {
            TargetDefinition = new Assignment(), 
            Condition = new ExpressionStatement() { Target = condition },
            Body = body, Callback = callback
        };
        expressionEvaluator.Evaluate(condition)
            .Returns(new EvaluatedResult { Type = DataType.Bool(), Value = true },
                new EvaluatedResult { Type = DataType.Bool(), Value = false });
        var strategy = new ForLoopInterpretationStrategy(forLoop, true, statementInterpreter, declarationInterpreter, expressionEvaluator);
        strategy.Apply();
        
        Received.InOrder(() =>
        {
            statementInterpreter.Interpret(body, true);
            expressionEvaluator.Evaluate(callback);
        });
    }
}

