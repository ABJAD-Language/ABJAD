using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Declarations;
using ABJAD.InterpretEngine.Shared.Expressions.Primitives;

namespace ABJAD.InterpretEngine.Declarations.Strategies;

public class ConstantDeclarationInterpretationStrategy : DeclarationInterpretationStrategy
{
    private readonly ConstantDeclaration constantDeclaration;
    private readonly ScopeFacade scope;

    public ConstantDeclarationInterpretationStrategy(ConstantDeclaration constantDeclaration, ScopeFacade scope)
    {
        this.constantDeclaration = constantDeclaration;
        this.scope = scope;
    }

    public void Apply()
    {
        ValidateReferenceDoesNotExistInScope();

        if (constantDeclaration.Type.IsNumber() && constantDeclaration.Value is NumberPrimitive number)
        {
            scope.DefineVariable(constantDeclaration.Name, constantDeclaration.Type, number.Value);
            return;
        }

        if (constantDeclaration.Type.IsString() && constantDeclaration.Value is StringPrimitive @string)
        {
            scope.DefineVariable(constantDeclaration.Name, constantDeclaration.Type, @string.Value);
            return;
        }

        if (constantDeclaration.Type.IsBool() && constantDeclaration.Value is BoolPrimitive @bool)
        {
            scope.DefineVariable(constantDeclaration.Name, constantDeclaration.Type, @bool.Value);
            return;
        }

        throw new ConstantDeclarationFailureException();
    }

    private void ValidateReferenceDoesNotExistInScope()
    {
        if (scope.ReferenceExistsInCurrentScope(constantDeclaration.Name))
        {
            throw new ReferenceAlreadyExistsException(constantDeclaration.Name);
        }
    }
}