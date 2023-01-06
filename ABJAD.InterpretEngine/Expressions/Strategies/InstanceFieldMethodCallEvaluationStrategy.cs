using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Expressions;

namespace ABJAD.InterpretEngine.Expressions.Strategies;

public class InstanceFieldMethodCallEvaluationStrategy : ExpressionEvaluationStrategy
{
    private readonly InstanceMethodCall instanceMethodCall;
    private readonly ScopeFacade scope;
    private readonly IExpressionEvaluator expressionEvaluator;
    private readonly IExpressionEvaluatorFactory expressionEvaluatorFactory;
    private readonly TextWriter writer;

    public InstanceFieldMethodCallEvaluationStrategy(InstanceMethodCall instanceMethodCall, ScopeFacade scope, IExpressionEvaluator expressionEvaluator, IExpressionEvaluatorFactory expressionEvaluatorFactory, TextWriter writer)
    {
        this.instanceMethodCall = instanceMethodCall;
        this.scope = scope;
        this.expressionEvaluator = expressionEvaluator;
        this.expressionEvaluatorFactory = expressionEvaluatorFactory;
        this.writer = writer;
    }

    public InstanceFieldMethodCallEvaluationStrategy(InstanceMethodCall instanceMethodCall, ScopeFacade scope, IExpressionEvaluator expressionEvaluator, TextWriter writer)
    {
        this.instanceMethodCall = instanceMethodCall;
        this.scope = scope;
        this.expressionEvaluator = expressionEvaluator;
        this.writer = writer;
        expressionEvaluatorFactory = new ExpressionEvaluatorFactory();
    }

    public EvaluatedResult Apply()
    {
        ValidateInstancesAreNotEmpty();
        var instance = instanceMethodCall.Instances.Count > 1 ? RetrieveChildInstance() : RetrieveInstance();
        
        if (instance is not InstanceElement instanceElement)
        {
            throw new NotInstanceReferenceException(instanceMethodCall.Instances.Last());
        }

        scope.AddScope(instanceElement.Scope);
        var localExpressionEvaluator = expressionEvaluatorFactory.NewExpressionEvaluator(scope, writer);

        var result = EvaluateMethodCall(localExpressionEvaluator);
        scope.RemoveLastScope();
        return result;
    }

    private EvaluatedResult EvaluateMethodCall(IExpressionEvaluator localExpressionEvaluator)
    {
        var methodCall = new MethodCall()
        {
            MethodName = instanceMethodCall.MethodName, 
            Arguments = instanceMethodCall.Arguments
        };
        return localExpressionEvaluator.Evaluate(methodCall);
    }

    private object RetrieveInstance()
    {
        var instanceName = instanceMethodCall.Instances[0];
        if (!scope.ReferenceExists(instanceName))
        {
            throw new ReferenceNameDoesNotExistException(instanceName);
        }

        return scope.GetReference(instanceName);
    }

    private object RetrieveChildInstance()
    {
        var instanceFieldAccess = new InstanceFieldAccess()
        {
            Instance = instanceMethodCall.Instances[0], 
            NestedFields = instanceMethodCall.Instances.Skip(1).ToList()
        };
        var evaluatedResult = expressionEvaluator.Evaluate(instanceFieldAccess);
        return evaluatedResult.Value;
    }

    private void ValidateInstancesAreNotEmpty()
    {
        if (instanceMethodCall.Instances.Count == 0)
        {
            throw new ArgumentException();
        }
    }
}