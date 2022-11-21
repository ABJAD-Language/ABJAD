using ABJAD.ParseEngine.Declarations;
using ABJAD.ParseEngine.Service.Expressions;
using ABJAD.ParseEngine.Service.Statements;

namespace ABJAD.ParseEngine.Service.Declarations;

public static class DeclarationsApiModelMapper
{
    public static DeclarationApiModel Map(Declaration declaration)
    {
        return declaration switch
        {
            ConstantDeclaration constantDeclaration => Map(constantDeclaration),
            VariableDeclaration variableDeclaration => Map(variableDeclaration),
            ClassDeclaration classDeclaration => Map(classDeclaration),
            FunctionDeclaration functionDeclaration => Map(functionDeclaration),
            ConstructorDeclaration constructorDeclaration => Map(constructorDeclaration)
        };
    }

    private static ConstructorDeclarationApiModel Map(ConstructorDeclaration declaration)
    {
        return new ConstructorDeclarationApiModel(declaration.Parameters.Select(Map).ToList(), StatementApiModelMapper.Map(declaration.Body));
    }

    private static FunctionDeclarationApiModel Map(FunctionDeclaration declaration)
    {
        return new FunctionDeclarationApiModel(declaration.Name, declaration.ReturnType,
            declaration.Parameters.Select(Map).ToList(), StatementApiModelMapper.Map(declaration.Body));
    }

    private static FunctionParameterApiModel Map(FunctionParameter parameter)
    {
        return new FunctionParameterApiModel() { Name = parameter.Name, Type = parameter.Type};
    }

    private static ClassDeclarationApiModel Map(ClassDeclaration declaration)
    {
        return new ClassDeclarationApiModel(declaration.Name, StatementApiModelMapper.Map(declaration.Body));
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