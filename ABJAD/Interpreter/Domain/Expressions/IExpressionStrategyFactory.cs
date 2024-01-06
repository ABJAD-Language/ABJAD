using ABJAD.Interpreter.Domain.Declarations;
using ABJAD.Interpreter.Domain.Expressions.Strategies;
using ABJAD.Interpreter.Domain.ScopeManagement;
using ABJAD.Interpreter.Domain.Shared.Expressions;
using ABJAD.Interpreter.Domain.Shared.Expressions.Assignments;
using ABJAD.Interpreter.Domain.Shared.Expressions.Binary;
using ABJAD.Interpreter.Domain.Shared.Expressions.Fixes;
using ABJAD.Interpreter.Domain.Shared.Expressions.Primitives;
using ABJAD.Interpreter.Domain.Shared.Expressions.Unary;
using ABJAD.Interpreter.Domain.Statements;

namespace ABJAD.Interpreter.Domain.Expressions;

public interface IExpressionStrategyFactory
{
    ExpressionEvaluationStrategy GetAssignmentEvaluationStrategy(AssignmentExpression expression, IExpressionEvaluator expressionEvaluator, ScopeFacade scopeFacade);
    ExpressionEvaluationStrategy GetBinaryExpressionEvaluationStrategy(BinaryExpression expression, IExpressionEvaluator expressionEvaluator);
    ExpressionEvaluationStrategy GetFixesEvaluationStrategy(FixExpression expression, ScopeFacade scopeFacade);
    ExpressionEvaluationStrategy GetPrimitiveEvaluationStrategy(Primitive primitive, ScopeFacade scopeFacade);
    ExpressionEvaluationStrategy GetUnaryExpressionEvaluationStrategy(UnaryExpression expression, IExpressionEvaluator expressionEvaluator);
    ExpressionEvaluationStrategy GetInstantiationEvaluationStrategy(Instantiation instantiation, ScopeFacade globalScope, ScopeFacade localScope, IExpressionEvaluator expressionEvaluator, IStatementInterpreter statementInterpreter, IDeclarationInterpreter declarationInterpreter);
    ExpressionEvaluationStrategy GetInstanceFieldMethodCallEvaluationStrategy(InstanceMethodCall instanceMethodCall, ScopeFacade scopeFacade, IExpressionEvaluator expressionEvaluator, TextWriter writer);
    ExpressionEvaluationStrategy GetInstanceFieldAccessEvaluationStrategy(InstanceFieldAccess instanceFieldAccess, ScopeFacade scopeFacade);
    ExpressionEvaluationStrategy GetMethodCallEvaluationStrategy(MethodCall methodCall, ScopeFacade scopeFacade, IExpressionEvaluator expressionEvaluator, IStatementInterpreter statementInterpreter, IDeclarationInterpreter declarationInterpreter);
}