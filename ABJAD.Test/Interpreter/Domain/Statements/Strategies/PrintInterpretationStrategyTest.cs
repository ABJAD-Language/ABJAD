using ABJAD.Interpreter.Domain;
using ABJAD.Interpreter.Domain.Expressions;
using ABJAD.Interpreter.Domain.Shared.Expressions;
using ABJAD.Interpreter.Domain.Shared.Statements;
using ABJAD.Interpreter.Domain.Statements.Strategies;
using ABJAD.Interpreter.Domain.Types;
using NSubstitute;

namespace ABJAD.Test.Interpreter.Domain.Statements.Strategies;

public class PrintInterpretationStrategyTest
{
    private readonly TextWriter textWriter = Substitute.For<TextWriter>();
    private readonly IExpressionEvaluator expressionEvaluator = Substitute.For<IExpressionEvaluator>();

    [Fact(DisplayName = "throws error when target has undefined value")]
    public void throws_error_when_target_has_undefined_value()
    {
        var print = new Print() { Target = Substitute.For<Expression>() };
        expressionEvaluator.Evaluate(print.Target).Returns(new EvaluatedResult { Value = SpecialValues.UNDEFINED });
        var strategy = new PrintInterpretationStrategy(print, textWriter, expressionEvaluator);
        Assert.Throws<OperationOnUndefinedValueException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "writes the value to the text writer")]
    public void writes_the_value_to_the_text_writer()
    {
        var print = new Print() { Target = Substitute.For<Expression>() };
        var value = new object();
        expressionEvaluator.Evaluate(print.Target).Returns(new EvaluatedResult { Type = Substitute.For<DataType>(), Value = value });
        var strategy = new PrintInterpretationStrategy(print, textWriter, expressionEvaluator);
        strategy.Apply();
        textWriter.Received(1).WriteLine(value);
    }

    [Fact(DisplayName = "returns a returning result with the flag set to false")]
    public void returns_a_returning_result_with_the_flag_set_to_false()
    {
        var print = new Print() { Target = Substitute.For<Expression>() };
        expressionEvaluator.Evaluate(print.Target).Returns(new EvaluatedResult { Type = Substitute.For<DataType>(), Value = new object() });
        var strategy = new PrintInterpretationStrategy(print, textWriter, expressionEvaluator);
        var result = strategy.Apply();
        Assert.False(result.Returned);
    }
}