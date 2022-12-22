﻿using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Statements.Strategies;
using ABJAD.InterpretEngine.Types;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Statements.Strategies;

public class IfElseInterpretationStrategyTest
{
    private readonly Interpreter<Statement> statementInterpreter = Substitute.For<Interpreter<Statement>>();
    private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();

    [Fact(DisplayName = "throws error if the main condition was not bool")]
    public void throws_error_if_the_main_condition_was_not_bool()
    {
        var condition = Substitute.For<Expression>();
        var ifElse = new IfElse() { MainConditional = new Conditional() { Condition = condition} };
        var conditionType = Substitute.For<DataType>();
        expressionEvaluator.Evaluate(condition).Returns(new EvaluatedResult { Type = conditionType });
        conditionType.IsBool().Returns(false);
        var strategy = new IfElseInterpretationStrategy(ifElse, statementInterpreter, expressionEvaluator);
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
        var strategy = new IfElseInterpretationStrategy(ifElse, statementInterpreter, expressionEvaluator);
        strategy.Apply();
        
        Received.InOrder(() =>
        {
            expressionEvaluator.Evaluate(mainCondition);
            statementInterpreter.Interpret(mainBody);
        });
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
        var strategy = new IfElseInterpretationStrategy(ifElse, statementInterpreter, expressionEvaluator);
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
        var strategy = new IfElseInterpretationStrategy(ifElse, statementInterpreter, expressionEvaluator);
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
        var strategy = new IfElseInterpretationStrategy(ifElse, statementInterpreter, expressionEvaluator);
        strategy.Apply();
        
        Received.InOrder(() =>
        {
            expressionEvaluator.Evaluate(mainCondition);
            expressionEvaluator.Evaluate(minorCondition);
            statementInterpreter.Interpret(minorBody);
        });
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
        var strategy = new IfElseInterpretationStrategy(ifElse, statementInterpreter, expressionEvaluator);
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
        var strategy = new IfElseInterpretationStrategy(ifElse, statementInterpreter, expressionEvaluator);
        strategy.Apply();
        
        Received.InOrder(() =>
        {
            expressionEvaluator.Evaluate(mainCondition);
            statementInterpreter.Interpret(elseBody);
        });
    }
}