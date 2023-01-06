using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;

namespace ABJAD.InterpretEngine.Statements.Strategies;

public class ExpressionStatementInterpretationStrategy : StatementInterpretationStrategy
{
    private readonly ExpressionStatement expressionStatement;
    private readonly IExpressionEvaluator expressionEvaluator;

    public ExpressionStatementInterpretationStrategy(ExpressionStatement expressionStatement, IExpressionEvaluator expressionEvaluator)
    {
        this.expressionStatement = expressionStatement;
        this.expressionEvaluator = expressionEvaluator;
    }

    public void Apply()
    {
        expressionEvaluator.Evaluate(expressionStatement.Target);
    }
}