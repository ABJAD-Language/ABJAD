using ABJAD.Interpreter.Domain;
using ABJAD.Interpreter.Domain.Expressions;
using ABJAD.Interpreter.Domain.Shared.Expressions;
using ABJAD.Interpreter.Domain.Shared.Statements;
using ABJAD.Interpreter.Domain.Statements;
using ABJAD.Interpreter.Domain.Statements.Strategies;
using ABJAD.Interpreter.Domain.Types;
using NSubstitute;

namespace ABJAD.Test.Interpreter.Domain.Statements.Strategies;

public class IfElseInterpretationStrategyTest
{
    private readonly IStatementInterpreter statementInterpreter = Substitute.For<IStatementInterpreter>();
    private readonly IExpressionEvaluator expressionEvaluator = Substitute.For<IExpressionEvaluator>();

    [Fact(DisplayName = "throws error if the main condition was not bool")]
    public void throws_error_if_the_main_condition_was_not_bool()
    {
        var condition = Substitute.For<Expression>();
        var ifElse = new IfElse() { MainConditional = new Conditional() { Condition = condition } };
        var conditionType = Substitute.For<DataType>();
        expressionEvaluator.Evaluate(condition).Returns(new EvaluatedResult { Type = conditionType });
        conditionType.IsBool().Returns(false);
        var strategy = new IfElseInterpretationStrategy(ifElse, false, statementInterpreter, expressionEvaluator);
        Assert.Throws<InvalidTypeException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "interpret the main body when the main condition evaluates to true")]
    public void interpret_the_main_body_when_the_main_condition_evaluates_to_true()
    {
        var mainCondition = Substitute.For<Expression>();
        var mainBody = Substitute.For<Statement>();
        var mainConditional = new Conditional() { Condition = mainCondition, Body = mainBody };
        expressionEvaluator.Evaluate(mainCondition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = true });

        var minorCondition = Substitute.For<Expression>();
        var minorBody = Substitute.For<Statement>();
        var minorConditional = new Conditional() { Condition = minorCondition, Body = minorBody };

        var elseBody = Substitute.For<Statement>();

        var ifElse = new IfElse()
        {
            MainConditional = mainConditional,
            OtherConditionals = new List<Conditional> { minorConditional },
            ElseBody = elseBody
        };
        var strategy = new IfElseInterpretationStrategy(ifElse, false, statementInterpreter, expressionEvaluator);
        strategy.Apply();

        Received.InOrder(() =>
        {
            expressionEvaluator.Evaluate(mainCondition);
            statementInterpreter.Interpret(mainBody);
        });
    }

    [Fact(DisplayName = "interpret the main body with function context set to true when it is the case")]
    public void interpret_the_main_body_with_function_context_set_to_true_when_it_is_the_case()
    {
        var mainCondition = Substitute.For<Expression>();
        var mainBody = Substitute.For<Statement>();
        var mainConditional = new Conditional() { Condition = mainCondition, Body = mainBody };
        expressionEvaluator.Evaluate(mainCondition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = true });

        var ifElse = new IfElse()
        {
            MainConditional = mainConditional,
            OtherConditionals = new List<Conditional>(),
        };
        var strategy = new IfElseInterpretationStrategy(ifElse, true, statementInterpreter, expressionEvaluator);
        strategy.Apply();

        Received.InOrder(() =>
        {
            expressionEvaluator.Evaluate(mainCondition);
            statementInterpreter.Interpret(mainBody, true);
        });
    }

    [Fact(DisplayName = "returns a returning result when the main body does so")]
    public void returns_a_returning_result_when_the_main_body_does_so()
    {
        var mainCondition = Substitute.For<Expression>();
        var mainBody = Substitute.For<Statement>();
        var mainConditional = new Conditional() { Condition = mainCondition, Body = mainBody };
        expressionEvaluator.Evaluate(mainCondition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = true });

        var evaluatedResult = new EvaluatedResult();
        statementInterpreter.Interpret(mainBody, true).Returns(StatementInterpretationResult.GetReturned(evaluatedResult));

        var ifElse = new IfElse()
        {
            MainConditional = mainConditional,
            OtherConditionals = new List<Conditional>(),
        };
        var strategy = new IfElseInterpretationStrategy(ifElse, true, statementInterpreter, expressionEvaluator);
        var result = strategy.Apply();

        Assert.True(result.Returned);
        Assert.True(result.IsValueReturned);
        Assert.Equal(evaluatedResult, result.ReturnedValue);
    }

    [Fact(DisplayName = "returns a non returning result when the main body does so")]
    public void returns_a_non_returning_result_when_the_main_body_does_so()
    {
        var mainCondition = Substitute.For<Expression>();
        var mainBody = Substitute.For<Statement>();
        var mainConditional = new Conditional() { Condition = mainCondition, Body = mainBody };
        expressionEvaluator.Evaluate(mainCondition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = true });

        var evaluatedResult = new EvaluatedResult();
        statementInterpreter.Interpret(mainBody, true).Returns(StatementInterpretationResult.GetNotReturned());

        var ifElse = new IfElse()
        {
            MainConditional = mainConditional,
            OtherConditionals = new List<Conditional>(),
        };
        var strategy = new IfElseInterpretationStrategy(ifElse, true, statementInterpreter, expressionEvaluator);
        var result = strategy.Apply();

        Assert.False(result.Returned);
    }

    [Fact(DisplayName = "checks the second condition when the main condition evaluates to false")]
    public void checks_the_second_condition_when_the_main_condition_evaluates_to_false()
    {
        var mainCondition = Substitute.For<Expression>();
        var mainBody = Substitute.For<Statement>();
        var mainConditional = new Conditional() { Condition = mainCondition, Body = mainBody };
        expressionEvaluator.Evaluate(mainCondition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = false });

        var minorCondition = Substitute.For<Expression>();
        expressionEvaluator.Evaluate(minorCondition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = false });
        var minorConditional = new Conditional() { Condition = minorCondition };

        var elseBody = Substitute.For<Statement>();

        var ifElse = new IfElse()
        {
            MainConditional = mainConditional,
            OtherConditionals = new List<Conditional> { minorConditional },
            ElseBody = elseBody
        };
        var strategy = new IfElseInterpretationStrategy(ifElse, false, statementInterpreter, expressionEvaluator);
        strategy.Apply();

        Received.InOrder(() =>
        {
            expressionEvaluator.Evaluate(mainCondition);
            expressionEvaluator.Evaluate(minorCondition);
        });
    }

    [Fact(DisplayName = "throws error if a minor condition is not a bool")]
    public void throws_error_if_a_minor_condition_is_not_a_bool()
    {
        var mainCondition = Substitute.For<Expression>();
        var mainBody = Substitute.For<Statement>();
        var mainConditional = new Conditional() { Condition = mainCondition, Body = mainBody };
        expressionEvaluator.Evaluate(mainCondition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = false });

        var minorCondition = Substitute.For<Expression>();
        var minorConditionType = Substitute.For<DataType>();
        minorConditionType.IsBool().Returns(false);
        expressionEvaluator.Evaluate(minorCondition).Returns(new EvaluatedResult { Type = minorConditionType });
        var minorConditional = new Conditional() { Condition = minorCondition };

        var elseBody = Substitute.For<Statement>();

        var ifElse = new IfElse()
        {
            MainConditional = mainConditional,
            OtherConditionals = new List<Conditional> { minorConditional },
            ElseBody = elseBody
        };
        var strategy = new IfElseInterpretationStrategy(ifElse, false, statementInterpreter, expressionEvaluator);
        Assert.Throws<InvalidTypeException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "interprets the body of the minor conditional when its condition evaluates to true")]
    public void interprets_the_body_of_the_minor_conditional_when_its_condition_evaluates_to_true()
    {
        var mainCondition = Substitute.For<Expression>();
        var mainBody = Substitute.For<Statement>();
        var mainConditional = new Conditional() { Condition = mainCondition, Body = mainBody };
        expressionEvaluator.Evaluate(mainCondition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = false });

        var minorCondition = Substitute.For<Expression>();
        var minorBody = Substitute.For<Statement>();
        expressionEvaluator.Evaluate(minorCondition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = true });
        var minorConditional = new Conditional() { Condition = minorCondition, Body = minorBody };

        var elseBody = Substitute.For<Statement>();

        var ifElse = new IfElse()
        {
            MainConditional = mainConditional,
            OtherConditionals = new List<Conditional> { minorConditional },
            ElseBody = elseBody
        };
        var strategy = new IfElseInterpretationStrategy(ifElse, false, statementInterpreter, expressionEvaluator);
        strategy.Apply();

        Received.InOrder(() =>
        {
            expressionEvaluator.Evaluate(mainCondition);
            expressionEvaluator.Evaluate(minorCondition);
            statementInterpreter.Interpret(minorBody);
        });
    }

    [Fact(DisplayName = "interprets the body of the second conditional with function context set to true when it is the case")]
    public void interprets_the_body_of_the_second_conditional_with_function_context_set_to_true_when_it_is_the_case()
    {
        var mainCondition = Substitute.For<Expression>();
        var mainConditional = new Conditional() { Condition = mainCondition };
        expressionEvaluator.Evaluate(mainCondition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = false });

        var minorCondition = Substitute.For<Expression>();
        var minorBody = Substitute.For<Statement>();
        expressionEvaluator.Evaluate(minorCondition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = true });
        var minorConditional = new Conditional() { Condition = minorCondition, Body = minorBody };

        var ifElse = new IfElse()
        {
            MainConditional = mainConditional,
            OtherConditionals = new List<Conditional> { minorConditional },
        };
        var strategy = new IfElseInterpretationStrategy(ifElse, true, statementInterpreter, expressionEvaluator);
        strategy.Apply();

        Received.InOrder(() =>
        {
            expressionEvaluator.Evaluate(mainCondition);
            expressionEvaluator.Evaluate(minorCondition);
            statementInterpreter.Interpret(minorBody, true);
        });
    }

    [Fact(DisplayName = "returns a retuning result when the body does so")]
    public void returns_a_retuning_result_when_the_body_does_so()
    {
        var mainCondition = Substitute.For<Expression>();
        var mainConditional = new Conditional() { Condition = mainCondition };
        expressionEvaluator.Evaluate(mainCondition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = false });

        var minorCondition = Substitute.For<Expression>();
        var minorBody = Substitute.For<Statement>();
        expressionEvaluator.Evaluate(minorCondition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = true });
        var minorConditional = new Conditional() { Condition = minorCondition, Body = minorBody };

        statementInterpreter.Interpret(minorBody, true).Returns(StatementInterpretationResult.GetReturned());

        var ifElse = new IfElse()
        {
            MainConditional = mainConditional,
            OtherConditionals = new List<Conditional> { minorConditional },
        };
        var strategy = new IfElseInterpretationStrategy(ifElse, true, statementInterpreter, expressionEvaluator);
        var result = strategy.Apply();
        Assert.True(result.Returned);
        Assert.False(result.IsValueReturned);
    }

    [Fact(DisplayName = "returns a non returning result when the body does so")]
    public void returns_a_non_returning_result_when_the_body_does_so()
    {
        var mainCondition = Substitute.For<Expression>();
        var mainConditional = new Conditional() { Condition = mainCondition };
        expressionEvaluator.Evaluate(mainCondition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = false });

        var minorCondition = Substitute.For<Expression>();
        var minorBody = Substitute.For<Statement>();
        expressionEvaluator.Evaluate(minorCondition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = true });
        var minorConditional = new Conditional() { Condition = minorCondition, Body = minorBody };

        statementInterpreter.Interpret(minorBody, true).Returns(StatementInterpretationResult.GetNotReturned());

        var ifElse = new IfElse()
        {
            MainConditional = mainConditional,
            OtherConditionals = new List<Conditional> { minorConditional },
        };
        var strategy = new IfElseInterpretationStrategy(ifElse, true, statementInterpreter, expressionEvaluator);
        var result = strategy.Apply();
        Assert.False(result.Returned);
    }

    [Fact(DisplayName = "interprets the else body when all the minor conditions evaluate to false")]
    public void interprets_the_else_body_when_all_the_minor_conditions_evaluate_to_false()
    {
        var mainCondition = Substitute.For<Expression>();
        var mainBody = Substitute.For<Statement>();
        var mainConditional = new Conditional() { Condition = mainCondition, Body = mainBody };
        expressionEvaluator.Evaluate(mainCondition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = false });

        var minorCondition = Substitute.For<Expression>();
        var minorBody = Substitute.For<Statement>();
        expressionEvaluator.Evaluate(minorCondition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = false });
        var minorConditional = new Conditional() { Condition = minorCondition, Body = minorBody };

        var elseBody = Substitute.For<Statement>();

        var ifElse = new IfElse()
        {
            MainConditional = mainConditional,
            OtherConditionals = new List<Conditional> { minorConditional },
            ElseBody = elseBody
        };
        var strategy = new IfElseInterpretationStrategy(ifElse, false, statementInterpreter, expressionEvaluator);
        strategy.Apply();

        Received.InOrder(() =>
        {
            expressionEvaluator.Evaluate(mainCondition);
            expressionEvaluator.Evaluate(minorCondition);
            statementInterpreter.Interpret(elseBody);
        });
    }

    [Fact(DisplayName = "interprets the else body when the main condition evaluates to false and there are no minor conditionals")]
    public void interprets_the_else_body_when_the_main_condition_evaluates_to_false_and_there_are_no_minor_conditionals()
    {
        var mainCondition = Substitute.For<Expression>();
        var mainBody = Substitute.For<Statement>();
        var mainConditional = new Conditional() { Condition = mainCondition, Body = mainBody };
        expressionEvaluator.Evaluate(mainCondition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = false });

        var elseBody = Substitute.For<Statement>();

        var ifElse = new IfElse { MainConditional = mainConditional, OtherConditionals = new List<Conditional>(), ElseBody = elseBody };
        var strategy = new IfElseInterpretationStrategy(ifElse, false, statementInterpreter, expressionEvaluator);
        strategy.Apply();

        Received.InOrder(() =>
        {
            expressionEvaluator.Evaluate(mainCondition);
            statementInterpreter.Interpret(elseBody);
        });
    }

    [Fact(DisplayName = "interprets the else body with function context set to true when it is the case")]
    public void interprets_the_else_body_with_function_context_set_to_true_when_it_is_the_case()
    {
        var mainCondition = Substitute.For<Expression>();
        var mainConditional = new Conditional() { Condition = mainCondition };
        expressionEvaluator.Evaluate(mainCondition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = false });

        var elseBody = Substitute.For<Statement>();

        var ifElse = new IfElse { MainConditional = mainConditional, OtherConditionals = new List<Conditional>(), ElseBody = elseBody };
        var strategy = new IfElseInterpretationStrategy(ifElse, true, statementInterpreter, expressionEvaluator);
        strategy.Apply();

        Received.InOrder(() =>
        {
            expressionEvaluator.Evaluate(mainCondition);
            statementInterpreter.Interpret(elseBody, true);
        });
    }

    [Fact(DisplayName = "returns a returning result when the else body does so")]
    public void returns_a_returning_result_when_the_else_body_does_so()
    {
        var mainCondition = Substitute.For<Expression>();
        var mainConditional = new Conditional() { Condition = mainCondition };
        expressionEvaluator.Evaluate(mainCondition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = false });

        var elseBody = Substitute.For<Statement>();
        statementInterpreter.Interpret(elseBody, true).Returns(StatementInterpretationResult.GetReturned());

        var ifElse = new IfElse { MainConditional = mainConditional, OtherConditionals = new List<Conditional>(), ElseBody = elseBody };
        var strategy = new IfElseInterpretationStrategy(ifElse, true, statementInterpreter, expressionEvaluator);
        var result = strategy.Apply();

        Assert.True(result.Returned);
        Assert.False(result.IsValueReturned);
    }

    [Fact(DisplayName = "returns a non returning result when the else body does so")]
    public void returns_a_non_returning_result_when_the_else_body_does_so()
    {
        var mainCondition = Substitute.For<Expression>();
        var mainConditional = new Conditional() { Condition = mainCondition };
        expressionEvaluator.Evaluate(mainCondition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = false });

        var elseBody = Substitute.For<Statement>();
        statementInterpreter.Interpret(elseBody, true).Returns(StatementInterpretationResult.GetNotReturned());

        var ifElse = new IfElse { MainConditional = mainConditional, OtherConditionals = new List<Conditional>(), ElseBody = elseBody };
        var strategy = new IfElseInterpretationStrategy(ifElse, true, statementInterpreter, expressionEvaluator);
        var result = strategy.Apply();

        Assert.False(result.Returned);
    }

    [Fact(DisplayName = "does not interpret anything when the main condition is false and there is no else body")]
    public void does_not_interpret_anything_when_the_main_condition_is_false_and_there_is_no_else_body()
    {
        var mainCondition = Substitute.For<Expression>();
        var mainBody = Substitute.For<Statement>();
        var mainConditional = new Conditional() { Condition = mainCondition, Body = mainBody };
        expressionEvaluator.Evaluate(mainCondition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = false });

        var ifElse = new IfElse { MainConditional = mainConditional, OtherConditionals = new List<Conditional>(), ElseBody = null };
        var strategy = new IfElseInterpretationStrategy(ifElse, false, statementInterpreter, expressionEvaluator);
        strategy.Apply();

        expressionEvaluator.Evaluate(mainCondition);
        statementInterpreter.DidNotReceiveWithAnyArgs().Interpret(Arg.Any<Statement>());
    }

    [Fact(DisplayName = "returns a non returning result when the main condition is false and there is no else body")]
    public void returns_a_non_returning_result_when_the_main_condition_is_false_and_there_is_no_else_body()
    {
        var mainCondition = Substitute.For<Expression>();
        var mainBody = Substitute.For<Statement>();
        var mainConditional = new Conditional() { Condition = mainCondition, Body = mainBody };
        expressionEvaluator.Evaluate(mainCondition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = false });

        var ifElse = new IfElse { MainConditional = mainConditional, OtherConditionals = new List<Conditional>(), ElseBody = null };
        var strategy = new IfElseInterpretationStrategy(ifElse, false, statementInterpreter, expressionEvaluator);
        var result = strategy.Apply();

        Assert.False(result.Returned);
    }
}