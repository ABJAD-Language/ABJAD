using ABJAD.InterpretEngine.Shared;
using ABJAD.InterpretEngine.Shared.Statements;

namespace ABJAD.InterpretEngine;

public class MainInterpreter
{
    private readonly Interpreter<Statement> statementInterpreter;

    public MainInterpreter(Interpreter<Statement> statementInterpreter)
    {
        this.statementInterpreter = statementInterpreter;
    }

    public void Interpret(List<Binding> bindings)
    {
        foreach (var binding in bindings)
        {
            if (binding is Statement statement)
            {
                statementInterpreter.Interpret(statement);   
            }
        }
    }
}