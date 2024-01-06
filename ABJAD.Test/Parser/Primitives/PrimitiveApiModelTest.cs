using ABJAD.Parser.Expressions.Primitives;

namespace ABJAD.Test.Parser.Primitives;

public class PrimitiveApiModelTest
{
    [Fact(DisplayName = "string primitive returns correct return type")]
    public void string_primitive_returns_correct_return_type()
    {
        Assert.Equal("expression.primitive.string", new StringPrimitiveApiModel("").Type);
    }

    [Fact(DisplayName = "bool primitive returns correct return type")]
    public void bool_primitive_returns_correct_return_type()
    {
        Assert.Equal("expression.primitive.bool", new BoolPrimitiveApiModel(true).Type);
    }

    [Fact(DisplayName = "identifier primitive returns correct return type")]
    public void identifier_primitive_returns_correct_return_type()
    {
        Assert.Equal("expression.primitive.identifier", new IdentifierPrimitiveApiModel("").Type);
    }

    [Fact(DisplayName = "number primitive returns correct return type")]
    public void number_primitive_returns_correct_return_type()
    {
        Assert.Equal("expression.primitive.number", new NumberPrimitiveApiModel(0).Type);
    }

    [Fact(DisplayName = "null primitive returns correct type")]
    public void null_primitive_returns_correct_type()
    {
        Assert.Equal("expression.primitive.null", new NullPrimitiveApiModel().Type);
    }


}