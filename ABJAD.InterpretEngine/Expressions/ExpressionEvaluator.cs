using ABJAD.InterpretEngine.Declarations;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions.Assignments;
using ABJAD.InterpretEngine.Shared.Expressions.Binary;
using ABJAD.InterpretEngine.Shared.Expressions.Fixes;
using ABJAD.InterpretEngine.Shared.Expressions.Primitives;
using ABJAD.InterpretEngine.Shared.Expressions.Unary;
using ABJAD.InterpretEngine.Statements;
using Environment = ABJAD.InterpretEngine.ScopeManagement.Environment;

namespace ABJAD.InterpretEngine.Expressions;

public class ExpressionEvaluator : Evaluator<Expression>
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
        this.expressionStrategyFactory = new ExpressionStrategyFactory();
        this.writer = writer;
    }

    public EvaluatedResult Evaluate(Expression target)
    {
        return target switch
        {
            AssignmentExpression expression => expressionStrategyFactory.GetAssignmentEvaluationStrategy(expression, this, scopeFacade).Apply(),
            BinaryExpression expression => expressionStrategyFactory.GetBinaryExpressionEvaluationStrategy(expression, this).Apply(),
            FixExpression expression => expressionStrategyFactory.GetFixesEvaluationStrategy(expression, scopeFacade).Apply(),
            UnaryExpression expression => expressionStrategyFactory.GetUnaryExpressionEvaluationStrategy(expression, this).Apply(),
            Primitive primitive => expressionStrategyFactory.GetPrimitiveEvaluationStrategy(primitive, scopeFacade).Apply(),
            Instantiation instantiation => HandleInstantiation(instantiation),
            _ => throw new ArgumentException()
        };
    }

    private EvaluatedResult HandleInstantiation(Instantiation instantiation)
    {
        var localScope = new Environment(new List<Scope>() { ScopeFactory.NewScope() });
        var globalScopeClone = scopeFacade.CloneScope();
        globalScopeClone.AddScope(localScope);
        var newStatementInterpreter = new StatementInterpreter(globalScopeClone, writer);
        var newDeclarationInterpreter = new DeclarationInterpreter(localScope, writer);
        var strategy = expressionStrategyFactory.GetInstantiationEvaluationStrategy(instantiation, globalScopeClone, localScope, this, newStatementInterpreter, newDeclarationInterpreter);
        return strategy.Apply();
    }
}