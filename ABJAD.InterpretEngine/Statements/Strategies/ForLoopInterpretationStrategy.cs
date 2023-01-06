using ABJAD.InterpretEngine.Declarations;
using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Shared.Declarations;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.Statements.Strategies;

public class ForLoopInterpretationStrategy : StatementInterpretationStrategy
{
    private readonly ForLoop forLoop;
    private readonly IStatementInterpreter statementInterpreter;
    private readonly IDeclarationInterpreter declarationInterpreter;
    private readonly IExpressionEvaluator expressionEvaluator;

    public ForLoopInterpretationStrategy(ForLoop forLoop, IStatementInterpreter statementInterpreter, IDeclarationInterpreter declarationInterpreter, IExpressionEvaluator expressionEvaluator)
    {
        this.forLoop = forLoop;
        this.statementInterpreter = statementInterpreter;
        this.declarationInterpreter = declarationInterpreter;
        this.expressionEvaluator = expressionEvaluator;
    }

    public void Apply()
    {
        HandleTargetDefinition();

        var condition = EvaluateCondition();
        while (condition)
        {
            statementInterpreter.Interpret(forLoop.Body);
            expressionEvaluator.Evaluate(forLoop.Callback);

            condition = EvaluateCondition();
        }
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