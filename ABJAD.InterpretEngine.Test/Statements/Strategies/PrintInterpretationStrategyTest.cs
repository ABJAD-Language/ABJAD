using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Statements.Strategies;
using ABJAD.InterpretEngine.Types;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Statements.Strategies;

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
}