using ABJAD.ParseEngine.Bindings;
using ABJAD.ParseEngine.Declarations;
using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Primitives;
using ABJAD.ParseEngine.Service.Bindings;
using ABJAD.ParseEngine.Service.Declarations;
using ABJAD.ParseEngine.Service.Expressions;
using ABJAD.ParseEngine.Service.Primitives;
using ABJAD.ParseEngine.Service.Statements;
using ABJAD.ParseEngine.Statements;
using FluentAssertions;

namespace ABJAD.ParseEngine.Service.Test.Bindings;

public class BindingApiModelMapperTest
{
    [Fact(DisplayName = "maps declaration binding correctly")]
    public void maps_declaration_binding_correctly()
    {
        var bindingApiModel = BindingApiModelMapper.Map(new DeclarationBinding(new VariableDeclaration("type", "name", null)));
        var expectedApiModel = new VariableDeclarationApiModel("type", "name");
        bindingApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps statement binding correctly")]
    public void maps_statement_binding_correctly()
    {
        var bindingApiModel = BindingApiModelMapper.Map(new StatementBinding(new ExpressionStatement(new PrimitiveExpression(StringPrimitive.From("value")))));
        var expectedApiModel = new ExpressionStatementApiModel(new PrimitiveExpressionApiModel(new StringPrimitiveApiModel("value")));
        bindingApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }
}