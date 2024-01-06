using ABJAD.Interpreter.Domain.Declarations;
using ABJAD.Interpreter.Domain.Expressions;
using ABJAD.Interpreter.Domain.Shared.Declarations;
using ABJAD.Interpreter.Domain.Shared.Statements;
using ABJAD.Interpreter.Domain.Types;

namespace ABJAD.Interpreter.Domain.Statements.Strategies;

public class ForLoopInterpretationStrategy : StatementInterpretationStrategy
{
    private readonly ForLoop forLoop;
    private readonly bool functionContext;
    private readonly IStatementInterpreter statementInterpreter;
    private readonly IDeclarationInterpreter declarationInterpreter;
    private readonly IExpressionEvaluator expressionEvaluator;

    public ForLoopInterpretationStrategy(ForLoop forLoop, bool functionContext, IStatementInterpreter statementInterpreter, IDeclarationInterpreter declarationInterpreter, IExpressionEvaluator expressionEvaluator)
    {
        this.forLoop = forLoop;
        this.statementInterpreter = statementInterpreter;
        this.declarationInterpreter = declarationInterpreter;
        this.expressionEvaluator = expressionEvaluator;
        this.functionContext = functionContext;
    }

    public StatementInterpretationResult Apply()
    {
        HandleTargetDefinition();

        var condition = EvaluateCondition();
        while (condition)
        {
            var result = statementInterpreter.Interpret(forLoop.Body, functionContext);
            if (result.Returned)
            {
                return result;
            }

            expressionEvaluator.Evaluate(forLoop.Callback);

            condition = EvaluateCondition();
        }

        return StatementInterpretationResult.GetNotReturned();
    }

    private bool EvaluateCondition()
    {
        var conditionEvaluatedResult = expressionEvaluator.Evaluate(forLoop.Condition.Target);
        if (!conditionEvaluatedResult.Type.IsBool())
        {
            throw new InvalidTypeException(conditionEvaluatedResult.Type, DataType.Bool());
        }

        return (bool)conditionEvaluatedResult.Value;
    }

    private void HandleTargetDefinition()
    {
        if (forLoop.TargetDefinition is VariableDeclaration declaration)
        {
            declarationInterpreter.Interpret(declaration);
        }
        else if (forLoop.TargetDefinition is Assignment assignment)
        {
            statementInterpreter.Interpret(assignment);
        }
        else
        {
            throw new ForLoopInvalidTargetDefinitionException();
        }
    }
}