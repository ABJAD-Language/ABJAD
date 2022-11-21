using ABJAD.ParseEngine.Declarations;
using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Primitives;
using ABJAD.ParseEngine.Service.Declarations;
using ABJAD.ParseEngine.Service.Expressions;
using ABJAD.ParseEngine.Service.Primitives;
using FluentAssertions;

namespace ABJAD.ParseEngine.Service.Test.Declarations;

public class DeclarationsApiModelMapperTest
{
    [Fact(DisplayName = "maps constant declaration correctly")]
    public void maps_constant_declaration_correctly()
    {
        var declarationApiModel = DeclarationsApiModelMapper.Map(new ConstantDeclaration("type", "name",
            new PrimitiveExpression(StringPrimitive.From("value"))));
        var expectedApiModel = new ConstantDeclarationApiModel("type", "name",
            new PrimitiveExpressionApiModel(new StringPrimitiveApiModel("value")));
        declarationApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }
    
    [Fact(DisplayName = "maps variable declaration correctly with instantiation correctly")]
    public void maps_variable_declaration_correctly_with_instantiation_correctly()
    {
        var declarationApiModel = DeclarationsApiModelMapper.Map(new VariableDeclaration("type", "name",
            new PrimitiveExpression(StringPrimitive.From("value"))));
        var expectedApiModel = new VariableDeclarationApiModel("type", "name",
            new PrimitiveExpressionApiModel(new StringPrimitiveApiModel("value")));
        declarationApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps variable declaration without instantiation correctly")]
    public void maps_variable_declaration_without_instantiation_correctly()
    {
        var declarationApiModel = DeclarationsApiModelMapper.Map(new VariableDeclaration("type", "name", null));
        var expectedApiModel = new VariableDeclarationApiModel("type", "name", null);
        declarationApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }
}