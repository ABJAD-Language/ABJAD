using ABJAD.Parser.Domain.Primitives;
using ABJAD.Parser.Expressions.Primitives;
using FluentAssertions;

namespace ABJAD.Test.Parser.Primitives;

public class PrimitiveApiModelMapperTest
{
    [Fact(DisplayName = "maps string primitive correctly")]
    public void maps_string_primitive_correctly()
    {
        var primitiveApiModel = PrimitiveApiModelMapper.Map(StringPrimitive.From("value"));
        var expectedApiModel = new StringPrimitiveApiModel("value");
        primitiveApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps number primitive correctly")]
    public void maps_number_primitive_correctly()
    {
        var primitiveApiModel = PrimitiveApiModelMapper.Map(NumberPrimitive.From("0"));
        var expectedApiModel = new NumberPrimitiveApiModel(0);
        primitiveApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps bool primitive correctly")]
    public void maps_bool_primitive_correctly()
    {
        var primitiveApiModel = PrimitiveApiModelMapper.Map(BoolPrimitive.False());
        var expectedApiModel = new BoolPrimitiveApiModel(false);
        primitiveApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps identifier primitive correctly")]
    public void maps_identifier_primitive_correctly()
    {
        var primitiveApiModel = PrimitiveApiModelMapper.Map(IdentifierPrimitive.From("id"));
        var expectedApiModel = new IdentifierPrimitiveApiModel("id");
        primitiveApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps null primitive correctly")]
    public void maps_null_primitive_correctly()
    {
        var primitiveApiModel = PrimitiveApiModelMapper.Map(NullPrimitive.Instance());
        var expectedApiModel = new NullPrimitiveApiModel();
        primitiveApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

}