using System;
using System.Collections.Generic;
using System.Linq;
using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Expressions.Unary;
using ABJAD.ParseEngine.Shared;
using FluentAssertions;
using Moq;
using Xunit;

namespace ABJAD.ParseEngine.Test.Expressions.Unary;

public class PostfixExpressionFactoryTest
{
    private readonly Mock<Expression> expressionMock = new();

    [Fact]
    private void PassingPlusPlusOperatorReturnsAdditionPostfix()
    {
        var expression = PostfixExpressionFactory.Get(expressionMock.Object, TokenType.PLUS_PLUS);
        var expectedExpression = new PostfixAdditionExpression(expressionMock.Object);

        AssertEqual(expectedExpression, expression);
    }

    [Fact]
    private void PassingDashDashOperatorReturnsSubtractionPostfix()
    {
        var expression = PostfixExpressionFactory.Get(expressionMock.Object, TokenType.DASH_DASH);
        var expectedExpression = new PostfixSubtractionExpression(expressionMock.Object);

        AssertEqual(expectedExpression, expression);
    }

    [Theory]
    [MemberData(nameof(GetNonPostfixOperatorTypes))]
    private void FailsIfOperatorTypeDoesNotPostfixOperations(TokenType operatorType)
    {
        Assert.Throws<InvalidPostfixSyntaxExpressionException>(() =>
            PostfixExpressionFactory.Get(expressionMock.Object, operatorType));
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