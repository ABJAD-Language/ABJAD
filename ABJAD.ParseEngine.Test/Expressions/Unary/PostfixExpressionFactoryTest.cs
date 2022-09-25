using System;
using System.Collections.Generic;
using System.Linq;
using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Expressions.Unary;
using ABJAD.ParseEngine.Expressions.Unary.Postfix;
using ABJAD.ParseEngine.Primitives;
using ABJAD.ParseEngine.Shared;
using FluentAssertions;
using Moq;
using Xunit;

namespace ABJAD.ParseEngine.Test.Expressions.Unary;

public class PostfixExpressionFactoryTest
{
    private readonly PrimitiveExpression identifierPrimitiveExpression = new(IdentifierPrimitive.From("id"));


    [Fact]
    private void FailsIfPassedExpressionThatIsNotPrimitiveExpression()
    {
        Assert.Throws<PostfixIllegalArgumentException>(() =>
            PostfixExpressionFactory.Get(new Mock<Expression>().Object, TokenType.PLUS_PLUS));
    }

    [Fact]
    private void FailsIfPassedPrimitiveExpressionThatIsNumberPrimitive()
    {
        var expression = new PrimitiveExpression(NumberPrimitive.From("2"));
        Assert.Throws<PostfixIllegalArgumentException>(() =>
            PostfixExpressionFactory.Get(expression, TokenType.PLUS_PLUS));
    }

    [Fact]
    private void FailsIfPassedPrimitiveExpressionThatIsStringPrimitive()
    {
        var expression = new PrimitiveExpression(StringPrimitive.From("2"));
        Assert.Throws<PostfixIllegalArgumentException>(() =>
            PostfixExpressionFactory.Get(expression, TokenType.PLUS_PLUS));
    }

    [Fact]
    private void FailsIfPassedPrimitiveExpressionThatIsBoolPrimitive()
    {
        var expression = new PrimitiveExpression(BoolPrimitive.True());
        Assert.Throws<PostfixIllegalArgumentException>(() =>
            PostfixExpressionFactory.Get(expression, TokenType.PLUS_PLUS));
    }

    [Fact]
    private void FailsIfPassedPrimitiveExpressionThatIsNullPrimitive()
    {
        var expression = new PrimitiveExpression(NullPrimitive.Instance());
        Assert.Throws<PostfixIllegalArgumentException>(() =>
            PostfixExpressionFactory.Get(expression, TokenType.PLUS_PLUS));
    }

    [Fact]
    private void PassingPlusPlusOperatorReturnsAdditionPostfix()
    {
        var expression = PostfixExpressionFactory.Get(identifierPrimitiveExpression, TokenType.PLUS_PLUS);
        var expectedExpression = new PostfixAdditionExpression(identifierPrimitiveExpression);

        AssertEqual(expectedExpression, expression);
    }

    [Fact]
    private void PassingDashDashOperatorReturnsSubtractionPostfix()
    {
        var expression = PostfixExpressionFactory.Get(identifierPrimitiveExpression, TokenType.DASH_DASH);
        var expectedExpression = new PostfixSubtractionExpression(identifierPrimitiveExpression);

        AssertEqual(expectedExpression, expression);
    }

    [Theory]
    [MemberData(nameof(GetNonPostfixOperatorTypes))]
    private void FailsIfOperatorTypeDoesNotPostfixOperations(TokenType operatorType)
    {
        Assert.Throws<InvalidPostfixSyntaxExpressionException>(() =>
            PostfixExpressionFactory.Get(identifierPrimitiveExpression, operatorType));
    }

    private static IEnumerable<object[]> GetNonPostfixOperatorTypes()
    {
        return Enum.GetValues<TokenType>()
            .Where(type => type != TokenType.PLUS_PLUS && type != TokenType.DASH_DASH)
            .Select(type => new object[] {type});
    }

    private static void AssertEqual(Expression expectedExpression, Expression expression)
    {
        expectedExpression
            .Should()
            .BeEquivalentTo(expression, options => options.RespectingRuntimeTypes());
    }
}