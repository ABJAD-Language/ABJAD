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

namespace ABJAD.Test.Parser.Bindings;

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
        var expectedApiModel = new ExpressionStatementApiModel(new StringPrimitiveApiModel("value"));
        bindingApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }
}