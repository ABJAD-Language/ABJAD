using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.Statements.Strategies;

public class WhileLoopInterpretationStrategy : StatementInterpretationStrategy
{
    private readonly WhileLoop whileLoop;
    private readonly IExpressionEvaluator expressionEvaluator;
    private readonly IStatementInterpreter statementInterpreter;

    public WhileLoopInterpretationStrategy(WhileLoop whileLoop, IExpressionEvaluator expressionEvaluator, IStatementInterpreter statementInterpreter)
    {
        this.whileLoop = whileLoop;
        this.expressionEvaluator = expressionEvaluator;
        this.statementInterpreter = statementInterpreter;
    }

    public void Apply()
    {
        var condition = EvaluateCondition();

        while (condition)
        {
            statementInterpreter.Interpret(whileLoop.Body);
            condition = EvaluateCondition();
        }
    }

    private bool EvaluateCondition()
    {
        var conditionEvaluatedResult = expressionEvaluator.Evaluate(whileLoop.Condition);
        if (!conditionEvaluatedResult.Type.IsBool())
        {
            throw new InvalidTypeException(conditionEvaluatedResult.Type, DataType.Bool());
        }

        return (bool)conditionEvaluatedResult.Value;
    }
}