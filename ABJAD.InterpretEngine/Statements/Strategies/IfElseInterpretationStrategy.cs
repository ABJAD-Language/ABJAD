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

    public void Apply()
    {
        // TODO handle case of return statement
        var mainCondition = EvaluateCondition(ifElse.MainConditional.Condition);
        if (mainCondition)
        {
            statementInterpreter.Interpret(ifElse.MainConditional.Body, functionContext);
            return;
        }
        
        foreach (var conditional in ifElse.OtherConditionals)
        {
            var condition = EvaluateCondition(conditional.Condition);
            if (condition)
            {
                statementInterpreter.Interpret(conditional.Body, functionContext);
                return;
            }
        }

        if (ifElse.ElseBody != null)
        {
            statementInterpreter.Interpret(ifElse.ElseBody, functionContext);
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