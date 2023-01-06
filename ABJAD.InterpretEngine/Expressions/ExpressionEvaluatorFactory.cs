using ABJAD.InterpretEngine.ScopeManagement;

namespace ABJAD.InterpretEngine.Expressions;

public class ExpressionEvaluatorFactory : IExpressionEvaluatorFactory
{
    public IExpressionEvaluator NewExpressionEvaluator(ScopeFacade scope, TextWriter writer)
    {
        return new ExpressionEvaluator(scope, writer);
    }
}