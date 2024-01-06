using ABJAD.Interpreter.Domain.Shared.Expressions;

namespace ABJAD.Interpreter.Domain.Expressions;

public interface IExpressionEvaluator
{
    EvaluatedResult Evaluate(Expression target);
}