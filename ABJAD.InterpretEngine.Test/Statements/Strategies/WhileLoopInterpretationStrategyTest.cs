using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Statements;
using ABJAD.InterpretEngine.Statements.Strategies;
using ABJAD.InterpretEngine.Types;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Statements.Strategies;

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
        var strategy = new WhileLoopInterpretationStrategy(whileLoop, expressionEvaluator, statementInterpreter);
        Assert.Throws<InvalidTypeException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "does not interprets body when the condition evaluates to false")]
    public void does_not_interprets_body_when_the_condition_evaluates_to_false()
    {
        var condition = Substitute.For<Expression>();
        var body = Substitute.For<Statement>();
        var whileLoop = new WhileLoop { Condition = condition, Body = body};
        expressionEvaluator.Evaluate(condition).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = false });
        var strategy = new WhileLoopInterpretationStrategy(whileLoop, expressionEvaluator, statementInterpreter);
        strategy.Apply();
        statementInterpreter.DidNotReceive().Interpret(body);
    }

    [Fact(DisplayName = "interprets body as long as the condition is true")]
    public void interprets_body_as_long_as_the_condition_is_true()
    {
        var condition = Substitute.For<Expression>();
        var body = Substitute.For<Statement>();
        var whileLoop = new WhileLoop { Condition = condition, Body = body};
        expressionEvaluator.Evaluate(condition).Returns(
            new EvaluatedResult { Type = DataType.Bool(), Value = true },
            new EvaluatedResult { Type = DataType.Bool(), Value = true },
            new EvaluatedResult { Type = DataType.Bool(), Value = false }
        );
        var strategy = new WhileLoopInterpretationStrategy(whileLoop, expressionEvaluator, statementInterpreter);
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
}