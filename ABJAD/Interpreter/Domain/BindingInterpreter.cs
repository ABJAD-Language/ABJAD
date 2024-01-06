using ABJAD.Interpreter.Domain.Declarations;
using ABJAD.Interpreter.Domain.Shared;
using ABJAD.Interpreter.Domain.Shared.Declarations;
using ABJAD.Interpreter.Domain.Shared.Statements;
using ABJAD.Interpreter.Domain.Statements;

namespace ABJAD.Interpreter.Domain;

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