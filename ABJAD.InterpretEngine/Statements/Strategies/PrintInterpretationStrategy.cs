using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;

namespace ABJAD.InterpretEngine.Statements.Strategies;

public class PrintInterpretationStrategy : StatementInterpretationStrategy
{
    private readonly Print print;
    private readonly TextWriter writer;
    private readonly Evaluator<Expression> expressionEvaluator;

    public PrintInterpretationStrategy(Print print, TextWriter writer, Evaluator<Expression> expressionEvaluator)
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
        
        writer.WriteLine(evaluatedResult.Value);
    }
}