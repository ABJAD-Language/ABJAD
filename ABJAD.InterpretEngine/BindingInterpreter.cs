using ABJAD.InterpretEngine.Declarations;
using ABJAD.InterpretEngine.Shared;
using ABJAD.InterpretEngine.Shared.Declarations;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Statements;

namespace ABJAD.InterpretEngine;

public class BindingInterpreter : Interpreter
{
    private readonly IStatementInterpreter statementInterpreter;
    private readonly IDeclarationInterpreter declarationInterpreter;

    public BindingInterpreter(IStatementInterpreter statementInterpreter, IDeclarationInterpreter declarationInterpreter)
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