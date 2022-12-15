using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;

namespace ABJAD.InterpretEngine;

public class StatementInterpreter : Interpreter<Statement>
{
    private readonly Evaluater<Expression> expressionEvaluater;

    public StatementInterpreter(Evaluater<Expression> expressionEvaluater)
    {
        this.expressionEvaluater = expressionEvaluater;
    }

    public void Interpret(Statement target)
    {
        if (target is ExpressionStatement expressionStatement)
        {
            expressionEvaluater.Evaluate(expressionStatement.Target);
        }
    }
}