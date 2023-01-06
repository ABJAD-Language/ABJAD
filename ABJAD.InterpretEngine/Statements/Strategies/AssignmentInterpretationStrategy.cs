using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.Statements.Strategies;

public class AssignmentInterpretationStrategy : StatementInterpretationStrategy
{
    private readonly Assignment assignment;
    private readonly ScopeFacade scopeFacade;
    private readonly IExpressionEvaluator expressionEvaluator;

    public AssignmentInterpretationStrategy(Assignment assignment, ScopeFacade scopeFacade, IExpressionEvaluator expressionEvaluator)
    {
        this.assignment = assignment;
        this.scopeFacade = scopeFacade;
        this.expressionEvaluator = expressionEvaluator;
    }

    public StatementInterpretationResult Apply()
    {
        ValidateTargetExists();

        var evaluatedResult = expressionEvaluator.Evaluate(assignment.Value);
        ValidateValueTypeMatchesTargetType(evaluatedResult);
        ValidateValueIsNotUndefined(evaluatedResult);

        scopeFacade.UpdateReference(assignment.Target, evaluatedResult.Value);

        return StatementInterpretationResult.GetNotReturned();
    }

    private static void ValidateValueIsNotUndefined(EvaluatedResult evaluatedResult)
    {
        if (evaluatedResult.Value.Equals(SpecialValues.UNDEFINED))
        {
            throw new OperationOnUndefinedValueException();
        }
    }

    private void ValidateValueTypeMatchesTargetType(EvaluatedResult evaluatedResult)
    {
        var targetType = scopeFacade.GetReferenceType(assignment.Target);
        if (!targetType.Is(evaluatedResult.Type))
        {
            throw new IncompatibleTypesException(targetType, evaluatedResult.Type);
        }
    }

    private void ValidateTargetExists()
    {
        if (!scopeFacade.ReferenceExists(assignment.Target))
        {
            throw new ReferenceNameDoesNotExistException(assignment.Target);
        }
    }
}