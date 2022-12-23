using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Declarations;

namespace ABJAD.InterpretEngine.Declarations.Strategies;

public class FunctionDeclarationInterpretationStrategy : DeclarationInterpretationStrategy
{
    private readonly FunctionDeclaration functionDeclaration;
    private readonly ScopeFacade scope;

    public FunctionDeclarationInterpretationStrategy(FunctionDeclaration functionDeclaration, ScopeFacade scope)
    {
        this.functionDeclaration = functionDeclaration;
        this.scope = scope;
    }

    public void Apply()
    {
        ValidateNoMatchingFunctionDeclarationExistsInSameScope();
        scope.DefineFunction(functionDeclaration.Name, BuildFunctionElement());
    }

    private FunctionElement BuildFunctionElement()
    {
        return new FunctionElement()
        {
            Body = functionDeclaration.Body,
            ReturnType = functionDeclaration.ReturnType,
            Parameters = functionDeclaration.Parameters.Select(MapParameter).ToList()
        };
    }

    private static FunctionParameter MapParameter(Parameter p)
    {
        return new FunctionParameter { Name = p.Name, Type = p.Type };
    }

    private void ValidateNoMatchingFunctionDeclarationExistsInSameScope()
    {
        if (scope.FunctionExistsInCurrentScope(functionDeclaration.Name, functionDeclaration.Parameters.Count))
        {
            throw new MatchingFunctionAlreadyExistsException(functionDeclaration.Name,
                functionDeclaration.Parameters.Count);
        }
    }
}