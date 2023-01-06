using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;

namespace ABJAD.InterpretEngine.Statements.Strategies;

public class ReturnInterpretationStrategy : StatementInterpretationStrategy
{
    private readonly Return statement;
    private readonly bool functionContext;
    private readonly IExpressionEvaluator expressionEvaluator;

    public ReturnInterpretationStrategy(Return statement, bool functionContext, IExpressionEvaluator expressionEvaluator)
    {
        this.statement = statement;
        this.functionContext = functionContext;
        this.expressionEvaluator = expressionEvaluator;
    }

    public StatementInterpretationResult Apply()
    {
        ValidateFunctionContext();

        if (statement.Target is null)
        {
            return StatementInterpretationResult.GetReturned();
        }

        var evaluatedResult = expressionEvaluator.Evaluate(statement.Target);

        return StatementInterpretationResult.GetReturned(evaluatedResult);
    }

    private void ValidateFunctionContext()
    {
        if (!functionContext)
        {
            throw new IllegalUseOfReturnStatementException();
        }
    }
}