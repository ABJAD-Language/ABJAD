using ABJAD.Parser.Bindings;
using ABJAD.Parser.Declarations;
using ABJAD.Parser.Domain.Bindings;
using ABJAD.Parser.Domain.Declarations;
using ABJAD.Parser.Domain.Expressions;
using ABJAD.Parser.Domain.Primitives;
using ABJAD.Parser.Domain.Statements;
using ABJAD.Parser.Expressions.Primitives;
using ABJAD.Parser.Statements;
using FluentAssertions;
using static ABJAD.Parser.Declarations.DeclarationsApiModelMapper;

namespace ABJAD.Test.Parser.Declarations;

public class DeclarationsApiModelMapperTest
{
    [Fact(DisplayName = "maps constant declaration correctly")]
    public void maps_constant_declaration_correctly()
    {
        var declarationApiModel = Map(new ConstantDeclaration("type", "name",
            new PrimitiveExpression(StringPrimitive.From("value"))));
        var expectedApiModel = new ConstantDeclarationApiModel("type", "name", new StringPrimitiveApiModel("value"));
        declarationApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps variable declaration correctly with instantiation correctly")]
    public void maps_variable_declaration_correctly_with_instantiation_correctly()
    {
        var declarationApiModel = Map(new VariableDeclaration("type", "name",
            new PrimitiveExpression(StringPrimitive.From("value"))));
        var expectedApiModel = new VariableDeclarationApiModel("type", "name",
            new StringPrimitiveApiModel("value"));
        declarationApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps variable declaration without instantiation correctly")]
    public void maps_variable_declaration_without_instantiation_correctly()
    {
        var declarationApiModel = Map(new VariableDeclaration("type", "name", null));
        var expectedApiModel = new VariableDeclarationApiModel("type", "name", null);
        declarationApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps block declaration correctly")]
    public void maps_block_declaration_correctly()
    {
        var declarationApiModel = Map(new BlockDeclaration(new List<DeclarationBinding>
            { new(new VariableDeclaration("string", "field", null)) }));
        var expectedApiModel = new BlockDeclarationApiModel(new List<DeclarationApiModel>()
            { new VariableDeclarationApiModel("string", "field") });
        declarationApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps class declaration correctly")]
    public void maps_class_declaration_correctly()
    {
        var declarationApiModel = Map(new ClassDeclaration("name", new BlockDeclaration(new List<DeclarationBinding>
                { new DeclarationBinding(new VariableDeclaration("string", "field", null)) })));
        var expectedApiModel = new ClassDeclarationApiModel("name", new BlockDeclarationApiModel(new List<DeclarationApiModel>
                { new VariableDeclarationApiModel("string", "field") }));
        declarationApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps function declaration correctly")]
    public void maps_function_declaration_correctly()
    {
        var declarationApiModel = Map(new FunctionDeclaration("name", "type",
            new List<FunctionParameter> { new("pType", "pName") }, new BlockStatement(new List<Binding>())));
        var expectedApiModel = new FunctionDeclarationApiModel("name", "type",
            new List<FunctionParameterApiModel> { new() { Name = "pName", ParameterType = "pType" } },
            new BlockStatementApiModel(new List<BindingApiModel>()));
        declarationApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps constructor declaration correctly")]
    public void maps_constructor_declaration_correctly()
    {
        var declarationApiModel = Map(new ConstructorDeclaration(new List<FunctionParameter> { new("pType", "pName") },
            new BlockStatement(new List<Binding>())));
        var expectedApiModel = new ConstructorDeclarationApiModel(new List<FunctionParameterApiModel> { new() { Name = "pName", ParameterType = "pType" } },
            new BlockStatementApiModel(new List<BindingApiModel>()));
        declarationApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }
}