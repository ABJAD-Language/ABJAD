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

public class ExpressionStrategyFactory : IExpressionStrategyFactory
{
    public ExpressionEvaluationStrategy GetAssignmentEvaluationStrategy(AssignmentExpression expression, IExpressionEvaluator expressionEvaluator, ScopeFacade scopeFacade)
    {
        return new AssignmentEvaluationStrategy(expression, scopeFacade, expressionEvaluator);
    }

    public ExpressionEvaluationStrategy GetBinaryExpressionEvaluationStrategy(BinaryExpression expression, IExpressionEvaluator expressionEvaluator)
    {
        return new BinaryExpressionEvaluationStrategy(expression, expressionEvaluator);
    }

    public ExpressionEvaluationStrategy GetFixesEvaluationStrategy(FixExpression expression, ScopeFacade scopeFacade)
    {
        return new FixesEvaluationStrategy(expression, scopeFacade);
    }

    public ExpressionEvaluationStrategy GetPrimitiveEvaluationStrategy(Primitive primitive, ScopeFacade scopeFacade)
    {
        return new PrimitiveEvaluationStrategy(primitive, scopeFacade);
    }

    public ExpressionEvaluationStrategy GetUnaryExpressionEvaluationStrategy(UnaryExpression expression,
        IExpressionEvaluator expressionEvaluator)
    {
        return new UnaryExpressionEvaluationStrategy(expression, expressionEvaluator);
    }

    public ExpressionEvaluationStrategy GetInstantiationEvaluationStrategy(Instantiation instantiation, ScopeFacade globalScope,
        ScopeFacade localScope, IExpressionEvaluator expressionEvaluator, IStatementInterpreter statementInterpreter,
        IDeclarationInterpreter declarationInterpreter)
    {
        return new InstantiationEvaluationStrategy(instantiation, globalScope, localScope, expressionEvaluator,
            statementInterpreter, declarationInterpreter);
    }

    public ExpressionEvaluationStrategy GetInstanceFieldMethodCallEvaluationStrategy(InstanceMethodCall instanceMethodCall,
        ScopeFacade scopeFacade, IExpressionEvaluator expressionEvaluator, TextWriter writer)
    {
        return new InstanceFieldMethodCallEvaluationStrategy(instanceMethodCall, scopeFacade, expressionEvaluator, writer);
    }

    public ExpressionEvaluationStrategy GetInstanceFieldAccessEvaluationStrategy(InstanceFieldAccess instanceFieldAccess,
        ScopeFacade scopeFacade)
    {
        return new InstanceFieldAccessEvaluationStrategy(instanceFieldAccess, scopeFacade);
    }

    public ExpressionEvaluationStrategy GetMethodCallEvaluationStrategy(MethodCall methodCall, ScopeFacade scopeFacade,
        IExpressionEvaluator expressionEvaluator, IStatementInterpreter statementInterpreter,
        IDeclarationInterpreter declarationInterpreter)
    {
        return new MethodCallEvaluationStrategy(methodCall, scopeFacade, expressionEvaluator, statementInterpreter, declarationInterpreter);
    }
}