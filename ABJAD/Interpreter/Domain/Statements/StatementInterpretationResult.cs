namespace ABJAD.Interpreter.Domain.Statements;

public class StatementInterpretationResult
{
    public bool Returned { get; init; }
    public EvaluatedResult? ReturnedValue { get; init; }

    public bool IsValueReturned => ReturnedValue != null;

    private StatementInterpretationResult() { }

    public static StatementInterpretationResult GetNotReturned()
    {
        return new StatementInterpretationResult() { Returned = false };
    }

    public static StatementInterpretationResult GetReturned()
    {
        return new StatementInterpretationResult() { Returned = true };
    }

    public static StatementInterpretationResult GetReturned(EvaluatedResult result)
    {
        return new StatementInterpretationResult() { Returned = true, ReturnedValue = result };
    }
}