using ABJAD.InterpretEngine.Shared;
using ABJAD.InterpretEngine.Shared.Statements;

namespace ABJAD.InterpretEngine;

public class MainInterpreter : Interpreter<Binding>
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
            Interpret(binding);
        }
    }

    public void Interpret(Binding target)
    {
        if (target is Statement statement)
        {
            statementInterpreter.Interpret(statement);   
        }
    }
}