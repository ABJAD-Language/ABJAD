namespace ABJAD.InterpretEngine;

public interface Evaluator<in T>
{
    EvaluatedResult Evaluate(T target);
}