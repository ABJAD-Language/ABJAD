using ABJAD.InterpretEngine.Shared.Expressions;

namespace ABJAD.InterpretEngine.Expressions;

public interface IExpressionEvaluator
{
    EvaluatedResult Evaluate(Expression target);
}