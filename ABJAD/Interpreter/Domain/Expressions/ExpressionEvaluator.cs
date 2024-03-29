﻿using ABJAD.Interpreter.Domain.Declarations;
using ABJAD.Interpreter.Domain.ScopeManagement;
using ABJAD.Interpreter.Domain.Shared.Expressions;
using ABJAD.Interpreter.Domain.Shared.Expressions.Assignments;
using ABJAD.Interpreter.Domain.Shared.Expressions.Binary;
using ABJAD.Interpreter.Domain.Shared.Expressions.Fixes;
using ABJAD.Interpreter.Domain.Shared.Expressions.Primitives;
using ABJAD.Interpreter.Domain.Shared.Expressions.Unary;
using ABJAD.Interpreter.Domain.Statements;
using Environment = ABJAD.Interpreter.Domain.ScopeManagement.Environment;

namespace ABJAD.Interpreter.Domain.Expressions;

public class ExpressionEvaluator : IExpressionEvaluator
{
    private readonly IExpressionStrategyFactory expressionStrategyFactory;
    private readonly ScopeFacade scopeFacade;
    private readonly TextWriter writer;

    public ExpressionEvaluator(IExpressionStrategyFactory expressionStrategyFactory, ScopeFacade scopeFacade, TextWriter writer)
    {
        this.expressionStrategyFactory = expressionStrategyFactory;
        this.scopeFacade = scopeFacade;
        this.writer = writer;
    }

    public ExpressionEvaluator(ScopeFacade scopeFacade, TextWriter writer)
    {
        this.scopeFacade = scopeFacade;
        expressionStrategyFactory = new ExpressionStrategyFactory();
        this.writer = writer;
    }

    public EvaluatedResult Evaluate(Expression target)
    {
        return target switch
        {
            AssignmentExpression expression => HandleAssignment(expression),
            BinaryExpression expression => HandleBinary(expression),
            FixExpression expression => HandleFix(expression),
            UnaryExpression expression => HandleUnary(expression),
            Primitive primitive => HandlePrimitive(primitive),
            Instantiation instantiation => HandleInstantiation(instantiation),
            MethodCall methodCall => HandleMethodCall(methodCall),
            InstanceFieldAccess instanceFieldAccess => HandleInstanceFieldAccess(instanceFieldAccess),
            InstanceMethodCall instanceMethodCall => HandleInstanceMethodCall(instanceMethodCall),
            _ => throw new ArgumentException()
        };
    }

    private EvaluatedResult HandleInstanceFieldAccess(InstanceFieldAccess instanceFieldAccess)
    {
        var strategy = expressionStrategyFactory.GetInstanceFieldAccessEvaluationStrategy(instanceFieldAccess, scopeFacade);
        return strategy.Apply();
    }

    private EvaluatedResult HandleInstanceMethodCall(InstanceMethodCall instanceMethodCall)
    {
        var strategy = expressionStrategyFactory.GetInstanceFieldMethodCallEvaluationStrategy(instanceMethodCall, scopeFacade, this, writer);
        return strategy.Apply();
    }

    private EvaluatedResult HandleAssignment(AssignmentExpression expression)
    {
        return expressionStrategyFactory.GetAssignmentEvaluationStrategy(expression, this, scopeFacade).Apply();
    }

    private EvaluatedResult HandleBinary(BinaryExpression expression)
    {
        return expressionStrategyFactory.GetBinaryExpressionEvaluationStrategy(expression, this).Apply();
    }

    private EvaluatedResult HandleFix(FixExpression expression)
    {
        return expressionStrategyFactory.GetFixesEvaluationStrategy(expression, scopeFacade).Apply();
    }

    private EvaluatedResult HandleUnary(UnaryExpression expression)
    {
        return expressionStrategyFactory.GetUnaryExpressionEvaluationStrategy(expression, this).Apply();
    }

    private EvaluatedResult HandlePrimitive(Primitive primitive)
    {
        return expressionStrategyFactory.GetPrimitiveEvaluationStrategy(primitive, scopeFacade).Apply();
    }

    private EvaluatedResult HandleMethodCall(MethodCall methodCall)
    {
        var strategy = expressionStrategyFactory.GetMethodCallEvaluationStrategy(methodCall, scopeFacade, this,
            GetStatementInterpreter(scopeFacade), GetDeclarationInterpreter(scopeFacade));
        return strategy.Apply();
    }

    private DeclarationInterpreter GetDeclarationInterpreter(ScopeFacade scope)
    {
        return new DeclarationInterpreter(scope, writer);
    }

    private StatementInterpreter GetStatementInterpreter(ScopeFacade scope)
    {
        return new StatementInterpreter(scope, writer);
    }

    private EvaluatedResult HandleInstantiation(Instantiation instantiation)
    {
        var localScope = new Environment(new List<Scope>() { ScopeFactory.NewScope() });
        scopeFacade.AddScope(localScope);
        var newStatementInterpreter = GetStatementInterpreter(scopeFacade);
        var newDeclarationInterpreter = GetDeclarationInterpreter(localScope);
        var strategy = expressionStrategyFactory.GetInstantiationEvaluationStrategy(instantiation, scopeFacade, localScope, this, newStatementInterpreter, newDeclarationInterpreter);
        var result = strategy.Apply();
        scopeFacade.RemoveLastScope();
        return result;
    }
}