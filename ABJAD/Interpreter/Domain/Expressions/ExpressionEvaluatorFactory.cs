using ABJAD.Interpreter.Domain.ScopeManagement;

namespace ABJAD.Interpreter.Domain.Expressions;

public class ExpressionEvaluatorFactory : IExpressionEvaluatorFactory
{
    public IExpressionEvaluator NewExpressionEvaluator(ScopeFacade scope, TextWriter writer)
    {
        return new ExpressionEvaluator(scope, writer);
    }
}