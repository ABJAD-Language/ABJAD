using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Expressions;

namespace ABJAD.InterpretEngine.Expressions;

public class ExpressionEvaluatorFactory : IExpressionEvaluatorFactory
{
    public Evaluator<Expression> NewExpressionEvaluator(ScopeFacade scope, TextWriter writer)
    {
        return new ExpressionEvaluator(scope, writer);
    }
}