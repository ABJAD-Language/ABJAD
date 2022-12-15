using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions.Assignments;

namespace ABJAD.InterpretEngine.Expressions.Strategies;

public class AssignmentInterpretingStrategy : ExpressionInterpretingStrategy
{
    private readonly AssignmentExpression assignmentExpression;
    private readonly IScope scope;
    private readonly Evaluater<Expression> expressionEvaluater;

    public AssignmentInterpretingStrategy(AssignmentExpression assignmentExpression, IScope scope, Evaluater<Expression> expressionEvaluater)
    {
        this.assignmentExpression = assignmentExpression;
        this.scope = scope;
        this.expressionEvaluater = expressionEvaluater;
    }

    public object Apply()
    {
        if (!scope.ReferenceExists(assignmentExpression.GetTarget()))
        {
            throw new ReferenceNameDoesNotExistException(assignmentExpression.GetTarget());
        }

        var oldValue = scope.Get(assignmentExpression.GetTarget());
        var offset = expressionEvaluater.Evaluate(assignmentExpression.GetValue());

        switch (assignmentExpression)
        {
            case AdditionAssignment:
                scope.Set(assignmentExpression.GetTarget(), (double)oldValue + (double)offset);
                return (double)oldValue + (double)offset;
            default: return null;
        }
    }
}