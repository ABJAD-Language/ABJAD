using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Declarations;
using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.Declarations.Strategies;

public class VariableDeclarationInterpretationStrategy : DeclarationInterpretationStrategy
{
    private readonly VariableDeclaration declaration;
    private readonly ScopeFacade scope;
    private readonly IExpressionEvaluator expressionEvaluator;

    public VariableDeclarationInterpretationStrategy(VariableDeclaration declaration, ScopeFacade scope, IExpressionEvaluator expressionEvaluator)
    {
        this.declaration = declaration;
        this.scope = scope;
        this.expressionEvaluator = expressionEvaluator;
    }

    public void Apply()
    {
        ValidateReferenceDoesNotExistsInScope();

        if (declaration.Value is null)
        {
            scope.DefineVariable(declaration.Name, declaration.Type, SpecialValues.UNDEFINED);
            return;
        }
        
        var evaluatedResult = expressionEvaluator.Evaluate(declaration.Value);
        ValidateTypeMatches(evaluatedResult);
        ValidateValueIsNotUndefined(evaluatedResult);
        ValidateVariableNullability(evaluatedResult);
        
        scope.DefineVariable(declaration.Name, declaration.Type, evaluatedResult.Value);
    }

    private void ValidateVariableNullability(EvaluatedResult evaluatedResult)
    {
        if (evaluatedResult.Value.Equals(SpecialValues.NULL) &&
            (evaluatedResult.Type.IsBool() || evaluatedResult.Type.IsNumber()))
        {
            throw new IllegalNullAssignmentException(declaration.Type);
        }
    }

    private static void ValidateValueIsNotUndefined(EvaluatedResult evaluatedResult)
    {
        if (evaluatedResult.Value.Equals(SpecialValues.UNDEFINED))
        {
            throw new OperationOnUndefinedValueException();
        }
    }

    private void ValidateTypeMatches(EvaluatedResult evaluatedResult)
    {
        if (!declaration.Type.Is(evaluatedResult.Type))
        {
            throw new IncompatibleTypesException(declaration.Type, evaluatedResult.Type);
        }
    }

    private void ValidateReferenceDoesNotExistsInScope()
    {
        if (scope.ReferenceExistsInCurrentScope(declaration.Name))
        {
            throw new ReferenceAlreadyExistsException(declaration.Name);
        }
    }
}