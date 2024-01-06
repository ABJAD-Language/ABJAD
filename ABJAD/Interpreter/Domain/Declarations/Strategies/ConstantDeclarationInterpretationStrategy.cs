using ABJAD.Interpreter.Domain.ScopeManagement;
using ABJAD.Interpreter.Domain.Shared.Declarations;
using ABJAD.Interpreter.Domain.Shared.Expressions.Primitives;

namespace ABJAD.Interpreter.Domain.Declarations.Strategies;

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
            scope.DefineConstant(constantDeclaration.Name, constantDeclaration.Type, number.Value);
            return;
        }

        if (constantDeclaration.Type.IsString())
        {
            if (constantDeclaration.Value is StringPrimitive @string)
            {
                scope.DefineConstant(constantDeclaration.Name, constantDeclaration.Type, @string.Value);
                return;
            }

            if (constantDeclaration.Value is NullPrimitive)
            {
                scope.DefineConstant(constantDeclaration.Name, constantDeclaration.Type, SpecialValues.NULL);
                return;
            }
        }

        if (constantDeclaration.Type.IsBool() && constantDeclaration.Value is BoolPrimitive @bool)
        {
            scope.DefineConstant(constantDeclaration.Name, constantDeclaration.Type, @bool.Value);
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