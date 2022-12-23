using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Declarations;

namespace ABJAD.InterpretEngine.Declarations.Strategies;

public class ClassDeclarationInterpretationStrategy : DeclarationInterpretationStrategy
{
    private readonly ClassDeclaration classDeclaration;
    private readonly ScopeFacade scope;

    public ClassDeclarationInterpretationStrategy(ClassDeclaration classDeclaration, ScopeFacade scope)
    {
        this.classDeclaration = classDeclaration;
        this.scope = scope;
    }

    public void Apply()
    {
        ValidateMatchingClassNameDoesNotExist();
        
        scope.DefineType(classDeclaration.Name, new ClassElement { Declarations = classDeclaration.Declarations });
    }

    private void ValidateMatchingClassNameDoesNotExist()
    {
        if (scope.TypeExists(classDeclaration.Name))
        {
            throw new MatchingClassDeclarationAlreadyExistsException(classDeclaration.Name);
        }
    }
}