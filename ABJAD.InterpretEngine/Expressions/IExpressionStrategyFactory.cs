using ABJAD.InterpretEngine.Declarations;
using ABJAD.InterpretEngine.Expressions.Strategies;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions.Assignments;
using ABJAD.InterpretEngine.Shared.Expressions.Binary;
using ABJAD.InterpretEngine.Shared.Expressions.Fixes;
using ABJAD.InterpretEngine.Shared.Expressions.Primitives;
using ABJAD.InterpretEngine.Shared.Expressions.Unary;
using ABJAD.InterpretEngine.Statements;

namespace ABJAD.InterpretEngine.Expressions;

public interface IExpressionStrategyFactory
{
    ExpressionEvaluationStrategy GetAssignmentEvaluationStrategy(AssignmentExpression expression, IExpressionEvaluator expressionEvaluator, ScopeFacade scopeFacade);
    ExpressionEvaluationStrategy GetBinaryExpressionEvaluationStrategy(BinaryExpression expression, IExpressionEvaluator expressionEvaluator);
    ExpressionEvaluationStrategy GetFixesEvaluationStrategy(FixExpression expression, ScopeFacade scopeFacade);
    ExpressionEvaluationStrategy GetPrimitiveEvaluationStrategy(Primitive primitive, ScopeFacade scopeFacade);
    ExpressionEvaluationStrategy GetUnaryExpressionEvaluationStrategy(UnaryExpression expression, IExpressionEvaluator expressionEvaluator);
    ExpressionEvaluationStrategy GetInstantiationEvaluationStrategy(Instantiation instantiation, ScopeFacade globalScope, ScopeFacade localScope,  IExpressionEvaluator expressionEvaluator, IStatementInterpreter statementInterpreter, IDeclarationInterpreter declarationInterpreter);
    ExpressionEvaluationStrategy GetInstanceFieldMethodCallEvaluationStrategy(InstanceMethodCall instanceMethodCall, ScopeFacade scopeFacade,  IExpressionEvaluator expressionEvaluator, TextWriter writer);
    ExpressionEvaluationStrategy GetInstanceFieldAccessEvaluationStrategy(InstanceFieldAccess instanceFieldAccess, ScopeFacade scopeFacade);
    ExpressionEvaluationStrategy GetMethodCallEvaluationStrategy(MethodCall methodCall, ScopeFacade scopeFacade,  IExpressionEvaluator expressionEvaluator, IStatementInterpreter statementInterpreter, IDeclarationInterpreter declarationInterpreter);
}