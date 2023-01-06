using ABJAD.InterpretEngine.ScopeManagement;

namespace ABJAD.InterpretEngine.Expressions;

public interface IExpressionEvaluatorFactory
{
    IExpressionEvaluator NewExpressionEvaluator(ScopeFacade scope, TextWriter writer);
}