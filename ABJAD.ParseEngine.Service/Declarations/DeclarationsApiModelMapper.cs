using ABJAD.ParseEngine.Declarations;
using ABJAD.ParseEngine.Service.Expressions;

namespace ABJAD.ParseEngine.Service.Declarations;

public static class DeclarationsApiModelMapper
{
    public static DeclarationApiModel Map(Declaration declaration)
    {
        return declaration switch
        {
            ConstantDeclaration constantDeclaration => Map(constantDeclaration),
            VariableDeclaration variableDeclaration => Map(variableDeclaration)
        };
    }

    private static VariableDeclarationApiModel Map(VariableDeclaration declaration)
    {
        var variableValue = declaration.Value != null ? ExpressionApiModelMapper.Map(declaration.Value) : null;
        return new VariableDeclarationApiModel(declaration.Type, declaration.Name, variableValue);
    }

    private static ConstantDeclarationApiModel Map(ConstantDeclaration declaration)
    {
        return new ConstantDeclarationApiModel(declaration.Type, declaration.Name, ExpressionApiModelMapper.Map(declaration.Value));
    }
}