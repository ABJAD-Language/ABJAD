using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;

namespace ABJAD.InterpretEngine.Statements.Strategies;

public class PrintInterpretationStrategy : StatementInterpretationStrategy
{
    private readonly Print print;
    private readonly TextWriter writer;
    private readonly IExpressionEvaluator expressionEvaluator;

    public PrintInterpretationStrategy(Print print, TextWriter writer, IExpressionEvaluator expressionEvaluator)
    {
        this.print = print;
        this.writer = writer;
        this.expressionEvaluator = expressionEvaluator;
    }

    public void Apply()
    {
        var evaluatedResult = expressionEvaluator.Evaluate(print.Target);
        if (evaluatedResult.Value.Equals(SpecialValues.UNDEFINED))
        {
            throw new OperationOnUndefinedValueException();
        }

        if (evaluatedResult.Type.IsBool())
        {
            writer.WriteLine((bool)evaluatedResult.Value ? "صحيح" : "خطأ");
        }
        else
        {
            writer.WriteLine(evaluatedResult.Value);
        }
    }
}