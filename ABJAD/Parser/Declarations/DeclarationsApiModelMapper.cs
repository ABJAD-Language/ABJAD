﻿using ABJAD.Parser.Domain.Declarations;
using ABJAD.Parser.Expressions;
using ABJAD.Parser.Statements;

namespace ABJAD.Parser.Declarations;

public static class DeclarationsApiModelMapper
{
    public static DeclarationApiModel Map(Declaration declaration)
    {
        return declaration switch
        {
            ConstantDeclaration constantDeclaration => Map(constantDeclaration),
            VariableDeclaration variableDeclaration => Map(variableDeclaration),
            BlockDeclaration blockDeclaration => Map(blockDeclaration),
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
        return new FunctionParameterApiModel() { Name = parameter.Name, ParameterType = parameter.Type };
    }

    private static ClassDeclarationApiModel Map(ClassDeclaration declaration)
    {
        return new ClassDeclarationApiModel(declaration.Name, Map(declaration.Body));
    }

    private static BlockDeclarationApiModel Map(BlockDeclaration declaration)
    {
        return new BlockDeclarationApiModel(declaration.DeclarationBindings.Select(db => Map(db.Declaration)).ToList());
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