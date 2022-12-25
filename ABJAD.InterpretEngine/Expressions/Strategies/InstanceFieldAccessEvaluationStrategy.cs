using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Expressions;

namespace ABJAD.InterpretEngine.Expressions.Strategies;

public class InstanceFieldAccessEvaluationStrategy : ExpressionEvaluationStrategy
{
    private readonly InstanceFieldAccess instanceFieldAccess;
    private readonly ScopeFacade scope;

    public InstanceFieldAccessEvaluationStrategy(InstanceFieldAccess instanceFieldAccess, ScopeFacade scope)
    {
        this.instanceFieldAccess = instanceFieldAccess;
        this.scope = scope;
    }

    public EvaluatedResult Apply()
    {
        ValidateListOfFieldsIsNotEmpty();
        ValidateInstanceExists(scope, instanceFieldAccess.Instance);

        var instance = GetReference(scope, instanceFieldAccess.Instance);
        string field = null;

        for (var index = 0; index < instanceFieldAccess.NestedFields.Count; index++)
        {
            field = instanceFieldAccess.NestedFields[index];
            ValidateInstanceExists(instance.Scope, field);

            if (index != instanceFieldAccess.NestedFields.Count - 1)
            {
                instance = GetReference(instance.Scope, field);
            }
        }
        
        var referenceType = instance.Scope.GetReferenceType(field);
        var reference = instance.Scope.GetReference(field);
        
        return new EvaluatedResult() { Type = referenceType, Value = reference };
    }

    private void ValidateListOfFieldsIsNotEmpty()
    {
        if (instanceFieldAccess.NestedFields.Count == 0)
        {
            throw new ArgumentException();
        }
    }

    private static InstanceElement GetReference(ScopeFacade scope, string reference)
    {
        if (scope.GetReference(reference) is not InstanceElement instance)
        {
            throw new NotInstanceReferenceException(reference);
        }

        return instance;
    }

    private static void ValidateInstanceExists(ScopeFacade scope, string instance)
    {
        if (!scope.ReferenceExists(instance))
        {
            throw new ReferenceNameDoesNotExistException(instance);
        }
    }
}