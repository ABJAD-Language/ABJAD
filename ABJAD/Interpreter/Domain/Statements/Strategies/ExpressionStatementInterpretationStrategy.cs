using ABJAD.Interpreter.Domain.Expressions;
using ABJAD.Interpreter.Domain.Shared.Statements;

namespace ABJAD.Interpreter.Domain.Statements.Strategies;

public class ExpressionStatementInterpretationStrategy : StatementInterpretationStrategy
{
    private readonly ExpressionStatement expressionStatement;
    private readonly IExpressionEvaluator expressionEvaluator;

    public ExpressionStatementInterpretationStrategy(ExpressionStatement expressionStatement, IExpressionEvaluator expressionEvaluator)
    {
        this.expressionStatement = expressionStatement;
        this.expressionEvaluator = expressionEvaluator;
    }

    public StatementInterpretationResult Apply()
    {
        expressionEvaluator.Evaluate(expressionStatement.Target);

        return StatementInterpretationResult.GetNotReturned();
    }
}