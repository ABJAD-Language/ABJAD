using ABJAD.InterpretEngine.Shared;
using ABJAD.InterpretEngine.Shared.Declarations;
using ABJAD.InterpretEngine.Shared.Statements;

namespace ABJAD.InterpretEngine;

public class BindingInterpreter : Interpreter<Binding>
{
    private readonly Interpreter<Statement> statementInterpreter;
    private readonly Interpreter<Declaration> declarationInterpreter;

    public BindingInterpreter(Interpreter<Statement> statementInterpreter, Interpreter<Declaration> declarationInterpreter)
    {
        this.statementInterpreter = statementInterpreter;
        this.declarationInterpreter = declarationInterpreter;
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
        else if (target is Declaration declaration)
        {
            declarationInterpreter.Interpret(declaration);
        }
        else
        {
            throw new ArgumentException();
        }
    }
}