using System;
using System.Collections.Generic;
using System.Linq;
using ABJAD.ParseEngine.Primitives;
using ABJAD.ParseEngine.Shared;
using FluentAssertions;
using Xunit;

namespace ABJAD.ParseEngine.Test.Primitives;

public class PrimitiveFactoryTest
{
    private static readonly List<TokenType> PrimitiveTokenTypes = new()
        {TokenType.ID, TokenType.NUMBER_CONST, TokenType.STRING_CONST, TokenType.NULL, TokenType.TRUE, TokenType.FALSE};

    [Fact]
    private void PassingNumberTokenReturnsNumberPrimitive()
    {
        var primitive = PrimitiveFactory.Get(new Token {Type = TokenType.NUMBER_CONST, Content = "5"});
        var expectedPrimitive = NumberPrimitive.From("5");

        AssertEqual(expectedPrimitive, primitive);
    }

    [Fact]
    private void PassingStringTokenReturnsStringPrimitive()
    {
        var primitive = PrimitiveFactory.Get(new Token {Type = TokenType.STRING_CONST, Content = "hello"});
        var expectedPrimitive = StringPrimitive.From("hello");

        AssertEqual(expectedPrimitive, primitive);
    }

    [Fact]
    private void PassingTrueTokenReturnsBoolPrimitiveOfValueTrue()
    {
        var primitive = PrimitiveFactory.Get(new Token {Type = TokenType.TRUE});
        var expectedPrimitive = BoolPrimitive.True();

        AssertEqual(expectedPrimitive, primitive);
    }

    [Fact]
    private void PassingFalseTokenReturnsBoolPrimitiveOfValueFalse()
    {
        var primitive = PrimitiveFactory.Get(new Token {Type = TokenType.FALSE});
        var expectedPrimitive = BoolPrimitive.False();

        AssertEqual(expectedPrimitive, primitive);
    }

    [Fact]
    private void PassingNullTokenReturnsNullPrimitive()
    {
        var primitive = PrimitiveFactory.Get(new Token {Type = TokenType.NULL});
        var expectedPrimitive = NullPrimitive.Instance();

        AssertEqual(expectedPrimitive, primitive);
    }

    [Fact]
    private void PassingIdTokenReturnsIdPrimitive()
    {
        var primitive = PrimitiveFactory.Get(new Token {Type = TokenType.ID, Content = "id"});
        var expectedPrimitive = IdentifierPrimitive.From("id");

        AssertEqual(expectedPrimitive, primitive);
    }

    [Theory]
    [MemberData(nameof(GetNonPrimitiveTokens))]
    private void FailsWhenPassingNonPrimitiveToken(Token token)
    {
        Assert.Throws<InvalidPrimitiveTypeException>(() => PrimitiveFactory.Get(token));
    }

    private static IEnumerable<object[]> GetNonPrimitiveTokens()
    {
        return Enum.GetValues<TokenType>()
            .Where(t => !PrimitiveTokenTypes.Contains(t))
            .Select(t => new object[] {new Token {Type = t}});
    }

    private static void AssertEqual(Primitive expectedPrimitive, Primitive primitive)
    {
        expectedPrimitive.Should().BeEquivalentTo(primitive, options => options.RespectingRuntimeTypes());
    }
}