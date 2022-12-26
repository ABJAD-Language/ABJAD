using ABJAD.InterpretEngine.Expressions.Strategies;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Declarations;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions.Assignments;
using ABJAD.InterpretEngine.Shared.Expressions.Binary;
using ABJAD.InterpretEngine.Shared.Expressions.Fixes;
using ABJAD.InterpretEngine.Shared.Expressions.Primitives;
using ABJAD.InterpretEngine.Shared.Expressions.Unary;
using ABJAD.InterpretEngine.Shared.Statements;

namespace ABJAD.InterpretEngine.Expressions;

public class ExpressionStrategyFactory : IExpressionStrategyFactory
{
    public ExpressionEvaluationStrategy GetAssignmentEvaluationStrategy(AssignmentExpression expression, Evaluator<Expression> expressionEvaluator, ScopeFacade scopeFacade)
    {
        return new AssignmentEvaluationStrategy(expression, scopeFacade, expressionEvaluator);
    }

    public ExpressionEvaluationStrategy GetBinaryExpressionEvaluationStrategy(BinaryExpression expression, Evaluator<Expression> expressionEvaluator)
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
        Evaluator<Expression> expressionEvaluator)
    {
        return new UnaryExpressionEvaluationStrategy(expression, expressionEvaluator);
    }

    public ExpressionEvaluationStrategy GetInstantiationEvaluationStrategy(Instantiation instantiation, ScopeFacade globalScope,
        ScopeFacade localScope, Evaluator<Expression> expressionEvaluator, Interpreter<Statement> statementInterpreter,
        Interpreter<Declaration> declarationInterpreter)
    {
        return new InstantiationEvaluationStrategy(instantiation, globalScope, localScope, expressionEvaluator,
            statementInterpreter, declarationInterpreter);
    }

    public ExpressionEvaluationStrategy GetInstanceFieldMethodCallEvaluationStrategy(InstanceMethodCall instanceMethodCall,
        ScopeFacade scopeFacade, Evaluator<Expression> expressionEvaluator, TextWriter writer)
    {
        return new InstanceFieldMethodCallEvaluationStrategy(instanceMethodCall, scopeFacade, expressionEvaluator, writer);
    }

    public ExpressionEvaluationStrategy GetInstanceFieldAccessEvaluationStrategy(InstanceFieldAccess instanceFieldAccess,
        ScopeFacade scopeFacade)
    {
        return new InstanceFieldAccessEvaluationStrategy(instanceFieldAccess, scopeFacade);
    }

    public ExpressionEvaluationStrategy GetMethodCallEvaluationStrategy(MethodCall methodCall, ScopeFacade scopeFacade,
        Evaluator<Expression> expressionEvaluator, Interpreter<Statement> statementInterpreter,
        Interpreter<Declaration> declarationInterpreter)
    {
        return new MethodCallEvaluationStrategy(methodCall, scopeFacade, expressionEvaluator, statementInterpreter, declarationInterpreter);
    }
}