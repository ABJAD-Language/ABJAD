using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.Statements.Strategies;

public class IfElseInterpretationStrategy : StatementInterpretationStrategy
{
    private readonly IfElse ifElse;
    private readonly bool functionContext;
    private readonly IStatementInterpreter statementInterpreter;
    private readonly IExpressionEvaluator expressionEvaluator;

    public IfElseInterpretationStrategy(IfElse ifElse, bool functionContext, IStatementInterpreter statementInterpreter, IExpressionEvaluator expressionEvaluator)
    {
        this.ifElse = ifElse;
        this.statementInterpreter = statementInterpreter;
        this.expressionEvaluator = expressionEvaluator;
        this.functionContext = functionContext;
    }

    public StatementInterpretationResult Apply()
    {
        var mainCondition = EvaluateCondition(ifElse.MainConditional.Condition);
        if (mainCondition)
        {
            return statementInterpreter.Interpret(ifElse.MainConditional.Body, functionContext);
        }
        
        foreach (var conditional in ifElse.OtherConditionals)
        {
            var condition = EvaluateCondition(conditional.Condition);
            if (condition)
            {
                return statementInterpreter.Interpret(conditional.Body, functionContext);
            }
        }

        if (ifElse.ElseBody != null)
        {
            return statementInterpreter.Interpret(ifElse.ElseBody, functionContext);
        }

        return StatementInterpretationResult.GetNotReturned();
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