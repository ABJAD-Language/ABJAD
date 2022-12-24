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

        AddClassToScope();

        AddConstructorsToScope();
    }

    private void AddConstructorsToScope()
    {
        classDeclaration.Declarations
            .Where(d => d is ConstructorDeclaration constructorDeclaration)
            .Select(d => d as ConstructorDeclaration)
            .ToList()
            .ForEach(AddConstructor!);
    }

    private void AddClassToScope()
    {
        var classElement = new ClassElement
        {
            Declarations = classDeclaration.Declarations.Where(d => d is not ConstructorDeclaration).ToList()
        };

        scope.DefineType(classDeclaration.Name, classElement);
    }

    private void ValidateMatchingClassNameDoesNotExist()
    {
        if (scope.TypeExists(classDeclaration.Name))
        {
            throw new MatchingClassDeclarationAlreadyExistsException(classDeclaration.Name);
        }
    }

    private void AddConstructor(ConstructorDeclaration constructorDeclaration)
    {
        var parameterTypes = constructorDeclaration.Parameters.Select(p => p.Type).ToArray();
        if (scope.TypeHasConstructor(classDeclaration.Name, parameterTypes))
        {
            throw new MatchingConstructorExistsException(classDeclaration.Name, parameterTypes);
        }
        
        scope.DefineTypeConstructor(classDeclaration.Name, BuildConstructor(constructorDeclaration));
    }

    private static ConstructorElement BuildConstructor(ConstructorDeclaration constructorDeclaration)
    {
        return new ConstructorElement()
        {
            Body = constructorDeclaration.Body,
            Parameters = constructorDeclaration.Parameters.Select(MapParameter).ToList()
        };
    }

    private static FunctionParameter MapParameter(Parameter p)
    {
        return new FunctionParameter() { Name = p.Name, Type = p.Type };
    }
}