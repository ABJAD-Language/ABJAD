using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;

namespace ABJAD.InterpretEngine.Statements.Strategies;

public class ExpressionStatementInterpretationStrategy : StatementInterpretationStrategy
{
    private readonly ExpressionStatement expressionStatement;
    private readonly Evaluator<Expression> expressionEvaluator;

    public ExpressionStatementInterpretationStrategy(ExpressionStatement expressionStatement, Evaluator<Expression> expressionEvaluator)
    {
        this.expressionStatement = expressionStatement;
        this.expressionEvaluator = expressionEvaluator;
    }

    public void Apply()
    {
        expressionEvaluator.Evaluate(expressionStatement.Target);
    }
}