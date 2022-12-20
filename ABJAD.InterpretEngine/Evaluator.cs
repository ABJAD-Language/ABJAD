using ABJAD.InterpretEngine.ScopeManagement;

namespace ABJAD.InterpretEngine;

public interface Evaluator<in T>
{
    EvaluatedResult Evaluate(T target);
    ScopeManager CloneScope();
}