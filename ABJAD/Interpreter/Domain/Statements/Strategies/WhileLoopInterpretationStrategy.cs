﻿using ABJAD.Interpreter.Domain.Expressions;
using ABJAD.Interpreter.Domain.Shared.Statements;
using ABJAD.Interpreter.Domain.Types;

namespace ABJAD.Interpreter.Domain.Statements.Strategies;

public class WhileLoopInterpretationStrategy : StatementInterpretationStrategy
{
    private readonly WhileLoop whileLoop;
    private readonly bool functionContext;
    private readonly IExpressionEvaluator expressionEvaluator;
    private readonly IStatementInterpreter statementInterpreter;

    public WhileLoopInterpretationStrategy(WhileLoop whileLoop, bool functionContext, IExpressionEvaluator expressionEvaluator, IStatementInterpreter statementInterpreter)
    {
        this.whileLoop = whileLoop;
        this.expressionEvaluator = expressionEvaluator;
        this.statementInterpreter = statementInterpreter;
        this.functionContext = functionContext;
    }

    public StatementInterpretationResult Apply()
    {
        var condition = EvaluateCondition();

        while (condition)
        {
            var result = statementInterpreter.Interpret(whileLoop.Body, functionContext);
            if (result.Returned)
            {
                return result;
            }
            condition = EvaluateCondition();
        }

        return StatementInterpretationResult.GetNotReturned();
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