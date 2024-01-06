using ABJAD.Interpreter.Domain.ScopeManagement;

namespace ABJAD.Interpreter.Domain.Expressions;

public interface IExpressionEvaluatorFactory
{
    IExpressionEvaluator NewExpressionEvaluator(ScopeFacade scope, TextWriter writer);
}