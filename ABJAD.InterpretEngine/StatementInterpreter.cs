using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;

namespace ABJAD.InterpretEngine;

public class StatementInterpreter : Interpreter<Statement>
{
    private readonly Interpreter<Expression> expressionInterpreter;

    public StatementInterpreter(Interpreter<Expression> expressionInterpreter)
    {
        this.expressionInterpreter = expressionInterpreter;
    }

    public void Interpret(Statement target)
    {
        if (target is ExpressionStatement expressionStatement)
        {
            expressionInterpreter.Interpret(expressionStatement.Target);
        }
    }
}