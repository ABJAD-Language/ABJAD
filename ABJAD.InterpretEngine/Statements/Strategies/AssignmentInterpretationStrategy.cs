using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.Statements.Strategies;

public class AssignmentInterpretationStrategy : StatementInterpretationStrategy
{
    private readonly Assignment assignment;
    private readonly IScope scope;
    private readonly Evaluator<Expression> expressionEvaluator;

    public AssignmentInterpretationStrategy(Assignment assignment, IScope scope, Evaluator<Expression> expressionEvaluator)
    {
        this.assignment = assignment;
        this.scope = scope;
        this.expressionEvaluator = expressionEvaluator;
    }

    public void Apply()
    {
        ValidateTargetExists();
        // TODO validate target is a variable

        var evaluatedResult = expressionEvaluator.Evaluate(assignment.Value);
        ValidateValueTypeMatchesTargetType(evaluatedResult);
        ValidateValueIsNotUndefined(evaluatedResult);

        scope.Set(assignment.Target, evaluatedResult.Value);
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
        var targetType = scope.GetType(assignment.Target);
        if (!targetType.Is(evaluatedResult.Type))
        {
            throw new IncompatibleTypesException(targetType, evaluatedResult.Type);
        }
    }

    private void ValidateTargetExists()
    {
        if (!scope.ReferenceExists(assignment.Target))
        {
            throw new ReferenceNameDoesNotExistException(assignment.Target);
        }
    }
}