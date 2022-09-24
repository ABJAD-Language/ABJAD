using System;
using System.Collections.Generic;
using System.Linq;
using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Expressions.Binary;
using ABJAD.ParseEngine.Shared;
using FluentAssertions;
using Moq;
using Xunit;

namespace ABJAD.ParseEngine.Test.Expressions.Binary;

public class BinaryOperationExpressionFactoryTest
{
    private readonly Mock<Expression> firstExpressionMock = new();
    private readonly Mock<Expression> secondExpressionMock = new();

    private static readonly List<TokenType> BinaryOperators = new()
    {
        TokenType.OR, TokenType.AND, TokenType.PLUS, TokenType.STAR, TokenType.SLASH, TokenType.DASH, TokenType.MODULO,
        TokenType.LESS_THAN, TokenType.LESS_EQUAL, TokenType.GREATER_THAN, TokenType.GREATER_EQUAL,
        TokenType.EQUAL_EQUAL, TokenType.BANG_EQUAL
    };

    [Fact]
    private void PassingOrOperatorReturnsOrExpression()
    {
        var expression = BinaryOperationExpressionFactory.BuildBinaryExpression(TokenType.OR,
            firstExpressionMock.Object,
            secondExpressionMock.Object);

        var expectedExpression = new OrOperationExpression(firstExpressionMock.Object, secondExpressionMock.Object);

        AssertEqual(expectedExpression, expression);
    }

    [Fact]
    private void PassingAndOperatorReturnsAndExpression()
    {
        var expression = BinaryOperationExpressionFactory.BuildBinaryExpression(TokenType.AND,
            firstExpressionMock.Object,
            secondExpressionMock.Object);

        var expectedExpression = new AndOperationExpression(firstExpressionMock.Object, secondExpressionMock.Object);

        AssertEqual(expectedExpression, expression);
    }

    [Fact]
    private void PassingEqualEqualOperatorReturnsEqualityCheckExpression()
    {
        var expression = BinaryOperationExpressionFactory.BuildBinaryExpression(TokenType.EQUAL_EQUAL,
            firstExpressionMock.Object,
            secondExpressionMock.Object);

        var expectedExpression = new EqualityCheckExpression(firstExpressionMock.Object, secondExpressionMock.Object);

        AssertEqual(expectedExpression, expression);
    }

    [Fact]
    private void PassingBangEqualOperatorReturnsInequalityCheckExpression()
    {
        var expression = BinaryOperationExpressionFactory.BuildBinaryExpression(TokenType.BANG_EQUAL,
            firstExpressionMock.Object,
            secondExpressionMock.Object);

        var expectedExpression = new InequalityCheckExpression(firstExpressionMock.Object, secondExpressionMock.Object);

        AssertEqual(expectedExpression, expression);
    }

    [Fact]
    private void PassingLessThanOperatorReturnsLessCheckExpression()
    {
        var expression = BinaryOperationExpressionFactory.BuildBinaryExpression(TokenType.LESS_THAN,
            firstExpressionMock.Object,
            secondExpressionMock.Object);

        var expectedExpression = new LessCheckExpression(firstExpressionMock.Object, secondExpressionMock.Object);

        AssertEqual(expectedExpression, expression);
    }

    [Fact]
    private void PassingLessEqualOperatorReturnsLessOrEqualCheckExpression()
    {
        var expression = BinaryOperationExpressionFactory.BuildBinaryExpression(TokenType.LESS_EQUAL,
            firstExpressionMock.Object,
            secondExpressionMock.Object);

        var expectedExpression =
            new LessOrEqualCheckExpression(firstExpressionMock.Object, secondExpressionMock.Object);

        AssertEqual(expectedExpression, expression);
    }

    [Fact]
    private void PassingGreaterThanOperatorReturnsGreaterCheckExpression()
    {
        var expression = BinaryOperationExpressionFactory.BuildBinaryExpression(TokenType.GREATER_THAN,
            firstExpressionMock.Object,
            secondExpressionMock.Object);

        var expectedExpression = new GreaterCheckExpression(firstExpressionMock.Object, secondExpressionMock.Object);

        AssertEqual(expectedExpression, expression);
    }

    [Fact]
    private void PassingGreaterEqualOperatorReturnsGreaterOrEqualCheckExpression()
    {
        var expression = BinaryOperationExpressionFactory.BuildBinaryExpression(TokenType.GREATER_EQUAL,
            firstExpressionMock.Object,
            secondExpressionMock.Object);

        var expectedExpression =
            new GreaterOrEqualCheckExpression(firstExpressionMock.Object, secondExpressionMock.Object);

        AssertEqual(expectedExpression, expression);
    }

    [Fact]
    private void PassingPlusOperatorReturnsAdditionExpression()
    {
        var expression = BinaryOperationExpressionFactory.BuildBinaryExpression(TokenType.PLUS,
            firstExpressionMock.Object,
            secondExpressionMock.Object);

        var expectedExpression = new AdditionExpression(firstExpressionMock.Object, secondExpressionMock.Object);

        AssertEqual(expectedExpression, expression);
    }

    [Fact]
    private void PassingDashOperatorReturnsSubtractionExpression()
    {
        var expression = BinaryOperationExpressionFactory.BuildBinaryExpression(TokenType.DASH,
            firstExpressionMock.Object,
            secondExpressionMock.Object);

        var expectedExpression = new SubtractionExpression(firstExpressionMock.Object, secondExpressionMock.Object);

        AssertEqual(expectedExpression, expression);
    }

    [Fact]
    private void PassingStarOperatorReturnsMultiplicationExpression()
    {
        var expression = BinaryOperationExpressionFactory.BuildBinaryExpression(TokenType.STAR,
            firstExpressionMock.Object,
            secondExpressionMock.Object);

        var expectedExpression = new MultiplicationExpression(firstExpressionMock.Object, secondExpressionMock.Object);

        AssertEqual(expectedExpression, expression);
    }

    [Fact]
    private void PassingSlashOperatorReturnsDivisionExpression()
    {
        var expression = BinaryOperationExpressionFactory.BuildBinaryExpression(TokenType.SLASH,
            firstExpressionMock.Object,
            secondExpressionMock.Object);

        var expectedExpression = new DivisionExpression(firstExpressionMock.Object, secondExpressionMock.Object);

        AssertEqual(expectedExpression, expression);
    }

    [Fact]
    private void PassingModuloOperatorReturnsModuloExpression()
    {
        var expression = BinaryOperationExpressionFactory.BuildBinaryExpression(TokenType.MODULO,
            firstExpressionMock.Object,
            secondExpressionMock.Object);

        var expectedExpression = new ModuloExpression(firstExpressionMock.Object, secondExpressionMock.Object);

        AssertEqual(expectedExpression, expression);
    }

    [Theory]
    [MemberData(nameof(GetBinaryOperatorTypes))]
    private void PassingBinaryOperatorDoesNotThrowException(TokenType operatorType)
    {
        var exception = Record.Exception(() => BinaryOperationExpressionFactory.BuildBinaryExpression(operatorType,
            firstExpressionMock.Object, secondExpressionMock.Object));
        Assert.Null(exception);
    }

    public static IEnumerable<object> GetBinaryOperatorTypes()
    {
        return BinaryOperators.Select(t => new object[] {t});
    }

    [Theory]
    [MemberData(nameof(GetNonBinaryOperatorTypes))]
    private void PassingAnyOtherOperatorThrowsException(TokenType operatorType)
    {
        Assert.Throws<ArgumentException>(() => BinaryOperationExpressionFactory.BuildBinaryExpression(operatorType,
            firstExpressionMock.Object, secondExpressionMock.Object));
    }

    public static IEnumerable<object> GetNonBinaryOperatorTypes()
    {
        return Enum.GetValues<TokenType>()
            .Where(t => !BinaryOperators.Contains(t))
            .Select(t => new object[] {t});
    }

    private static void AssertEqual(Expression expectedExpression, Expression expression)
    {
        expectedExpression.Should().BeEquivalentTo(expression, options => options.RespectingRuntimeTypes());
    }
}