using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Declarations;

namespace ABJAD.InterpretEngine.Declarations;

public class DeclarationInterpreter : Interpreter<Declaration>
{
    private readonly ScopeFacade scope;

    public DeclarationInterpreter(ScopeFacade scope)
    {
        this.scope = scope;
    }

    public void Interpret(Declaration target)
    {
        throw new NotImplementedException();
    }
}