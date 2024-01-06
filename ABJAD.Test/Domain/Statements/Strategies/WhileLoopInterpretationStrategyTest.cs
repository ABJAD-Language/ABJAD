using ABJAD.Interpreter.Domain;
using ABJAD.Interpreter.Domain.Expressions;
using ABJAD.Interpreter.Domain.Shared.Expressions;
using ABJAD.Interpreter.Domain.Shared.Statements;
using ABJAD.Interpreter.Domain.Statements;
using ABJAD.Interpreter.Domain.Statements.Strategies;
using ABJAD.Interpreter.Domain.Types;
using NSubstitute;

namespace ABJAD.Test.Domain.Statements.Strategies;

public class WhileLoopInterpretationStrategyTest
{
    private readonly IExpressionEvaluator expressionEvaluator = Substitute.For<IExpressionEvaluator>();
    private readonly IStatementInterpreter statementInterpreter = Substitute.For<IStatementInterpreter>();

    [Fact(DisplayName = "throws error if the condition is not of type bool")]
    public void throws_error_if_the_condition_is_not_of_type_bool()
    {
        var condition = Substitute.For<Expression>();
        var whileLoop = new WhileLoop { Condition = condition };
        var conditionType = Substitute.For<DataType>();
        expressionEvaluator.Evaluate(condition).Returns(new EvaluatedResult { Type = conditionType });
        conditionType.IsBool().Returns(false);
        var strategy = new WhileLoopInterpretationStrategy(whileLoop, false, expressionEvaluator, statementInterpreter);
        Assert.Throws<InvalidTypeException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "does not interprets body when the condition evaluates to false")]
    public void does_not_interprets_body_when_the_condition_evaluates_to_false()
    {
        var condition = Substitute.For<Expression>();
        var body = Substitute.For<Statement>();
        var whileLoop = new WhileLoop { Condition = condition, Body = body };
        expressionEvaluator.Evaluate(condition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = false });
        var strategy = new WhileLoopInterpretationStrategy(whileLoop, false, expressionEvaluator, statementInterpreter);
        strategy.Apply();
        statementInterpreter.DidNotReceive().Interpret(body);
    }

    [Fact(DisplayName = "interprets body as long as the condition is true")]
    public void interprets_body_as_long_as_the_condition_is_true()
    {
        var condition = Substitute.For<Expression>();
        var body = Substitute.For<Statement>();
        var whileLoop = new WhileLoop { Condition = condition, Body = body };
        expressionEvaluator.Evaluate(condition).Returns(
            new EvaluatedResult { Type = DataType.Bool(), Value = true },
            new EvaluatedResult { Type = DataType.Bool(), Value = true },
            new EvaluatedResult { Type = DataType.Bool(), Value = false }
        );
        statementInterpreter.Interpret(body).Returns(StatementInterpretationResult.GetNotReturned());
        var strategy = new WhileLoopInterpretationStrategy(whileLoop, false, expressionEvaluator, statementInterpreter);
        strategy.Apply();
        Received.InOrder(() =>
        {
            expressionEvaluator.Evaluate(condition);
            statementInterpreter.Interpret(body);
            expressionEvaluator.Evaluate(condition);
            statementInterpreter.Interpret(body);
            expressionEvaluator.Evaluate(condition);
        });
    }

    [Fact(DisplayName = "interprets the body with function context set to true when it is the case")]
    public void interprets_the_body_with_function_context_set_to_true_when_it_is_the_case()
    {
        var condition = Substitute.For<Expression>();
        var body = Substitute.For<Statement>();
        var whileLoop = new WhileLoop { Condition = condition, Body = body };
        expressionEvaluator.Evaluate(condition).Returns(
            new EvaluatedResult { Type = DataType.Bool(), Value = true },
            new EvaluatedResult { Type = DataType.Bool(), Value = false }
        );
        statementInterpreter.Interpret(body, true).Returns(StatementInterpretationResult.GetNotReturned());
        var strategy = new WhileLoopInterpretationStrategy(whileLoop, true, expressionEvaluator, statementInterpreter);
        strategy.Apply();
        Received.InOrder(() =>
        {
            expressionEvaluator.Evaluate(condition);
            statementInterpreter.Interpret(body, true);
            expressionEvaluator.Evaluate(condition);
        });
    }

    [Fact(DisplayName = "returns a returning result and stops the loop when the statement returns a returning result")]
    public void returns_a_returning_result_and_stops_the_loop_when_the_statement_returns_a_returning_result()
    {
        var condition = Substitute.For<Expression>();
        var body = Substitute.For<Statement>();
        var whileLoop = new WhileLoop { Condition = condition, Body = body };
        expressionEvaluator.Evaluate(condition).Returns(
            new EvaluatedResult { Type = DataType.Bool(), Value = true },
            new EvaluatedResult { Type = DataType.Bool(), Value = true },
            new EvaluatedResult { Type = DataType.Bool(), Value = false }
        );
        statementInterpreter.Interpret(body, true).Returns(StatementInterpretationResult.GetReturned());
        var strategy = new WhileLoopInterpretationStrategy(whileLoop, true, expressionEvaluator, statementInterpreter);
        var result = strategy.Apply();

        statementInterpreter.Received(1).Interpret(body, true);
        expressionEvaluator.Received(1).Evaluate(condition);

        Assert.True(result.Returned);
        Assert.False(result.IsValueReturned);
    }

    [Fact(DisplayName = "returns a non returning result if the loop finishes normally")]
    public void returns_a_non_returning_result_if_the_loop_finishes_normally()
    {
        var condition = Substitute.For<Expression>();
        var body = Substitute.For<Statement>();
        var whileLoop = new WhileLoop { Condition = condition, Body = body };
        expressionEvaluator.Evaluate(condition).Returns(
            new EvaluatedResult { Type = DataType.Bool(), Value = true },
            new EvaluatedResult { Type = DataType.Bool(), Value = true },
            new EvaluatedResult { Type = DataType.Bool(), Value = false }
        );
        statementInterpreter.Interpret(body, true).Returns(StatementInterpretationResult.GetNotReturned());
        var strategy = new WhileLoopInterpretationStrategy(whileLoop, true, expressionEvaluator, statementInterpreter);
        var result = strategy.Apply();

        statementInterpreter.Received(2).Interpret(body, true);
        expressionEvaluator.Received(3).Evaluate(condition);

        Assert.False(result.Returned);
    }
}