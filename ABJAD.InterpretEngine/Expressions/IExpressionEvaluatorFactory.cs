using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Expressions;

namespace ABJAD.InterpretEngine.Expressions;

public interface IExpressionEvaluatorFactory
{
    Evaluator<Expression> NewExpressionEvaluator(ScopeFacade scope, TextWriter writer);
}