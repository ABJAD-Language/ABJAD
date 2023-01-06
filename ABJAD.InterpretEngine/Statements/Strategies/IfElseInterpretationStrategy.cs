using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.Statements.Strategies;

public class IfElseInterpretationStrategy : StatementInterpretationStrategy
{
    private readonly IfElse ifElse;
    private readonly IStatementInterpreter statementInterpreter;
    private readonly IExpressionEvaluator expressionEvaluator;

    public IfElseInterpretationStrategy(IfElse ifElse, IStatementInterpreter statementInterpreter, IExpressionEvaluator expressionEvaluator)
    {
        this.ifElse = ifElse;
        this.statementInterpreter = statementInterpreter;
        this.expressionEvaluator = expressionEvaluator;
    }

    public void Apply()
    {
        var mainCondition = EvaluateCondition(ifElse.MainConditional.Condition);
        if (mainCondition)
        {
            statementInterpreter.Interpret(ifElse.MainConditional.Body);
            return;
        }
        
        foreach (var conditional in ifElse.OtherConditionals)
        {
            var condition = EvaluateCondition(conditional.Condition);
            if (condition)
            {
                statementInterpreter.Interpret(conditional.Body);
                return;
            }
        }

        if (ifElse.ElseBody != null)
        {
            statementInterpreter.Interpret(ifElse.ElseBody);
        }
    }

    private bool EvaluateCondition(Expression conditionExpression)
    {
        var evaluatedResult = expressionEvaluator.Evaluate(conditionExpression);
        if (!evaluatedResult.Type.IsBool())
        {
            throw new InvalidTypeException(evaluatedResult.Type, DataType.Bool());
        }

        var mainCondition = (bool)evaluatedResult.Value;
        return mainCondition;
    }
}