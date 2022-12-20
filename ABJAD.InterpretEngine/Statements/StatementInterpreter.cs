using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;

namespace ABJAD.InterpretEngine.Statements;

public class StatementInterpreter : Interpreter<Statement>
{
    private readonly Evaluator<Expression> expressionEvaluator;

    public StatementInterpreter(Evaluator<Expression> expressionEvaluator)
    {
        this.expressionEvaluator = expressionEvaluator;
    }

    public void Interpret(Statement target)
    {
        if (target is ExpressionStatement expressionStatement)
        {
            expressionEvaluator.Evaluate(expressionStatement.Target);
        }
    }
}