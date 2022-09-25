using System;
using System.Collections.Generic;
using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Expressions.Binary;
using ABJAD.ParseEngine.Expressions.Unary;
using ABJAD.ParseEngine.Primitives;
using ABJAD.ParseEngine.Shared;
using FluentAssertions;
using Xunit;

namespace ABJAD.ParseEngine.Test.Expressions;

public class ParsingExpressionStrategyTest
{
    [Fact]
    private void FailsIfTokensAreNull()
    {
        var strategy = new ParsingExpressionStrategy();
        Assert.Throws<ArgumentNullException>(() => strategy.Parse(null, 0));
    }

    [Fact]
    private void FailsIfTokensAreEmpty()
    {
        var strategy = new ParsingExpressionStrategy();
        Assert.Throws<ArgumentException>(() => strategy.Parse(new List<Token>(), 0));
    }

    [Fact]
    private void FailsIfTokensWhereOnlyWhitespaces()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.WHITE_SPACE},
        };
        var strategy = new ParsingExpressionStrategy();
        Assert.Throws<ArgumentException>(() => strategy.Parse(tokens, 0));
    }

    [Fact]
    private void FailsIfIndexIsNegative()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.NUMBER_CONST, Content = "3"},
        };
        var strategy = new ParsingExpressionStrategy();
        Assert.Throws<ArgumentException>(() => strategy.Parse(tokens, -1));
    }

    /// <summary>
    /// parsing expression 3 || 4
    /// </summary>
    [Fact]
    public void ParsingOrReturnsOrExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.NUMBER_CONST, Content = "3"},
            new() {Type = TokenType.OR},
            new() {Type = TokenType.NUMBER_CONST, Content = "4"}
        };

        var expectedExpression = new AndOperationExpression(
            new PrimitiveExpression(NumberPrimitive.From("3")),
            new PrimitiveExpression(NumberPrimitive.From("4"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    [Fact]
    private void ThrowsExceptionWhenSecondOperandOfOrWasNotFound()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.NUMBER_CONST, Content = "3"},
            new() {Type = TokenType.OR, Line = 4, Index = 2}
        };

        AssertFails<Exception>(tokens);
        // var exception = AssertFails<MissingExpressionException>(tokens);
        // Assert.Equal("Expected expression was not found at line 4:3", exception.EnglishMessage);
    }

    [Fact]
    private void ThrowsExceptionWhenFirstOperandOfOrWasNotFound()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.OR, Line = 4, Index = 2},
            new() {Type = TokenType.NUMBER_CONST, Content = "3"}
        };

        Assert.NotNull(Record.Exception(() => new ParsingExpressionStrategy().Parse(tokens, 0)));
        // AssertFails<FailedToParseExpressionException>(tokens);
    }

    /// <summary>
    /// parsing expression true && false
    /// </summary>
    [Fact]
    private void ParsingAndReturnsAndExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.TRUE},
            new() {Type = TokenType.AND},
            new() {Type = TokenType.FALSE}
        };

        var expectedExpression = new AndOperationExpression(
            new PrimitiveExpression(BoolPrimitive.True()),
            new PrimitiveExpression(BoolPrimitive.False())
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    [Fact]
    private void ThrowsExceptionWhenSecondOperandOfAndWasNotFound()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.NUMBER_CONST, Content = "3"},
            new() {Type = TokenType.AND, Line = 4, Index = 2}
        };

        AssertFails<Exception>(tokens);
        // var exception = AssertFails<MissingExpressionException>(tokens);
        // Assert.Equal("Expected expression was not found at line 4:3", exception.EnglishMessage);
    }

    [Fact]
    private void ThrowsExceptionWhenFirstOperandOfAndWasNotFound()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.AND, Line = 4, Index = 2},
            new() {Type = TokenType.NUMBER_CONST, Content = "3"}
        };

        Assert.NotNull(Record.Exception(() => new ParsingExpressionStrategy().Parse(tokens, 0)));
        // AssertFails<FailedToParseExpressionException>(tokens);
    }

    /// <summary>
    /// parsing expression var1 || var2 && var3
    /// </summary>
    [Fact]
    private void ParsingAndAndOrExpressionsGivesThePrecedenceForTheAndWhenItComessSecond()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.ID, Content = "var1"},
            new() {Type = TokenType.OR},
            new() {Type = TokenType.ID, Content = "var2"},
            new() {Type = TokenType.AND},
            new() {Type = TokenType.ID, Content = "var3"}
        };

        var expectedExpression = new OrOperationExpression(
            new PrimitiveExpression(IdentifierPrimitive.From("var1")),
            new AndOperationExpression(
                new PrimitiveExpression(IdentifierPrimitive.From("var2")),
                new PrimitiveExpression(IdentifierPrimitive.From("var3"))
            )
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression var1 && var2 || var3
    /// </summary>
    [Fact]
    private void ParsingAndAndOrExpressionsGivesThePrecedenceForTheAndWhenItComesFirst()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.ID, Content = "var1"},
            new() {Type = TokenType.AND},
            new() {Type = TokenType.ID, Content = "var2"},
            new() {Type = TokenType.OR},
            new() {Type = TokenType.ID, Content = "var3"}
        };

        var expectedExpression = new OrOperationExpression(
            new AndOperationExpression(
                new PrimitiveExpression(IdentifierPrimitive.From("var1")),
                new PrimitiveExpression(IdentifierPrimitive.From("var2"))
            ),
            new PrimitiveExpression(IdentifierPrimitive.From("var3"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression var1 == var2
    /// </summary>
    [Fact]
    private void ParsingEqualityReturnsEqualityExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.ID, Content = "var1"},
            new() {Type = TokenType.EQUAL_EQUAL},
            new() {Type = TokenType.ID, Content = "var2"}
        };

        var expectedExpression = new EqualityCheckExpression(
            new PrimitiveExpression(IdentifierPrimitive.From("var1")),
            new PrimitiveExpression(IdentifierPrimitive.From("var2"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    [Fact]
    private void ThrowsExceptionWhenSecondOperandOfEqualityWasNotFound()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.NUMBER_CONST, Content = "3"},
            new() {Type = TokenType.EQUAL_EQUAL, Line = 4, Index = 2}
        };

        AssertFails<Exception>(tokens);
        // var exception = AssertFails<MissingExpressionException>(tokens);
        // Assert.Equal("Expected expression was not found at line 4:3", exception.EnglishMessage);
    }

    [Fact]
    private void ThrowsExceptionWhenFirstOperandOfEqualityWasNotFound()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.AND, Line = 4, Index = 2},
            new() {Type = TokenType.EQUAL_EQUAL, Content = "3"}
        };

        Assert.NotNull(Record.Exception(() => new ParsingExpressionStrategy().Parse(tokens, 0)));
        // AssertFails<FailedToParseExpressionException>(tokens);
    }

    /// <summary>
    /// parsing expression var1 != var2
    /// </summary>
    [Fact]
    private void ParsingInequalityReturnsInequalityExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.ID, Content = "var1"},
            new() {Type = TokenType.BANG_EQUAL},
            new() {Type = TokenType.ID, Content = "var2"}
        };

        var expectedExpression = new InequalityCheckExpression(
            new PrimitiveExpression(IdentifierPrimitive.From("var1")),
            new PrimitiveExpression(IdentifierPrimitive.From("var2"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression var1 && var2 == var3
    /// </summary>
    [Fact]
    private void ParsingAndAndEqualityExpressionsGivesThePrecedenceForTheEqualityWhenItComesFirst()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.ID, Content = "var1"},
            new() {Type = TokenType.AND},
            new() {Type = TokenType.ID, Content = "var2"},
            new() {Type = TokenType.EQUAL_EQUAL},
            new() {Type = TokenType.ID, Content = "var3"}
        };

        var expectedExpression = new AndOperationExpression(
            new PrimitiveExpression(IdentifierPrimitive.From("var1")),
            new EqualityCheckExpression(
                new PrimitiveExpression(IdentifierPrimitive.From("var2")),
                new PrimitiveExpression(IdentifierPrimitive.From("var3"))
            )
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    [Fact]
    private void ThrowsExceptionWhenTwoOperatorsAreFoundNextToEachOther()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.ID, Content = "var1"},
            new() {Type = TokenType.AND},
            new() {Type = TokenType.EQUAL_EQUAL},
            new() {Type = TokenType.ID, Content = "var2"}
        };

        Assert.NotNull(Record.Exception(() => new ParsingExpressionStrategy().Parse(tokens, 0)));
        // AssertFails<FailedToParseExpressionException>(tokens);
    }

    /// <summary>
    /// parsing expression var1 != var2 == var3
    /// </summary>
    [Fact]
    private void ParsingInequalityAndEqualityExpressionsDoesNotGiveThePrecedenceForAny()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.ID, Content = "var1"},
            new() {Type = TokenType.BANG_EQUAL},
            new() {Type = TokenType.ID, Content = "var2"},
            new() {Type = TokenType.EQUAL_EQUAL},
            new() {Type = TokenType.ID, Content = "var3"}
        };

        var expectedExpression = new EqualityCheckExpression(
            new InequalityCheckExpression(
                new PrimitiveExpression(IdentifierPrimitive.From("var1")),
                new PrimitiveExpression(IdentifierPrimitive.From("var2"))
            ),
            new PrimitiveExpression(IdentifierPrimitive.From("var3"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression 3 < 4
    /// </summary>
    [Fact]
    public void ParsingLessThanReturnsLessThanExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.NUMBER_CONST, Content = "3"},
            new() {Type = TokenType.LESS_THAN},
            new() {Type = TokenType.NUMBER_CONST, Content = "4"}
        };

        var expectedExpression = new LessCheckExpression(
            new PrimitiveExpression(NumberPrimitive.From("3")),
            new PrimitiveExpression(NumberPrimitive.From("4"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression 3 <= 4
    /// </summary>
    [Fact]
    public void ParsingLessOrEqualThanReturnsLessOrEqualThanExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.NUMBER_CONST, Content = "3"},
            new() {Type = TokenType.LESS_EQUAL},
            new() {Type = TokenType.NUMBER_CONST, Content = "4"}
        };

        var expectedExpression = new LessOrEqualCheckExpression(
            new PrimitiveExpression(NumberPrimitive.From("3")),
            new PrimitiveExpression(NumberPrimitive.From("4"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression 3 > 4
    /// </summary>
    [Fact]
    public void ParsingGreaterThanReturnsGreaterThanExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.NUMBER_CONST, Content = "3"},
            new() {Type = TokenType.GREATER_THAN},
            new() {Type = TokenType.NUMBER_CONST, Content = "4"}
        };

        var expectedExpression = new GreaterCheckExpression(
            new PrimitiveExpression(NumberPrimitive.From("3")),
            new PrimitiveExpression(NumberPrimitive.From("4"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression 3 >= 4
    /// </summary>
    [Fact]
    public void ParsingGreaterOrEqualThanReturnsGreaterOrEqualThanExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.NUMBER_CONST, Content = "3"},
            new() {Type = TokenType.GREATER_EQUAL},
            new() {Type = TokenType.NUMBER_CONST, Content = "4"}
        };

        var expectedExpression = new GreaterOrEqualCheckExpression(
            new PrimitiveExpression(NumberPrimitive.From("3")),
            new PrimitiveExpression(NumberPrimitive.From("4"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression 3 + 4
    /// </summary>
    [Fact]
    public void ParsingAdditionReturnsAdditionExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.NUMBER_CONST, Content = "3"},
            new() {Type = TokenType.PLUS},
            new() {Type = TokenType.NUMBER_CONST, Content = "4"}
        };

        var expectedExpression = new AdditionExpression(
            new PrimitiveExpression(NumberPrimitive.From("3")),
            new PrimitiveExpression(NumberPrimitive.From("4"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression 3 - 4
    /// </summary>
    [Fact]
    public void ParsingSubtractReturnsSubtractionExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.NUMBER_CONST, Content = "3"},
            new() {Type = TokenType.DASH},
            new() {Type = TokenType.NUMBER_CONST, Content = "4"}
        };

        var expectedExpression = new SubtractionExpression(
            new PrimitiveExpression(NumberPrimitive.From("3")),
            new PrimitiveExpression(NumberPrimitive.From("4"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression 3 * 4
    /// </summary>
    [Fact]
    public void ParsingMultiplicationReturnsMultiplicationExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.NUMBER_CONST, Content = "3"},
            new() {Type = TokenType.STAR},
            new() {Type = TokenType.NUMBER_CONST, Content = "4"}
        };

        var expectedExpression = new MultiplicationExpression(
            new PrimitiveExpression(NumberPrimitive.From("3")),
            new PrimitiveExpression(NumberPrimitive.From("4"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsnig expression: 3 / 4
    /// </summary>
    [Fact]
    public void ParsingDivisionReturnsDivisionExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.NUMBER_CONST, Content = "3"},
            new() {Type = TokenType.SLASH},
            new() {Type = TokenType.NUMBER_CONST, Content = "4"}
        };

        var expectedExpression = new DivisionExpression(
            new PrimitiveExpression(NumberPrimitive.From("3")),
            new PrimitiveExpression(NumberPrimitive.From("4"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression: 3 % 4
    /// </summary>
    [Fact]
    public void ParsingModuloReturnsModuloExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.NUMBER_CONST, Content = "3"},
            new() {Type = TokenType.MODULO},
            new() {Type = TokenType.NUMBER_CONST, Content = "4"}
        };

        var expectedExpression = new ModuloExpression(
            new PrimitiveExpression(NumberPrimitive.From("3")),
            new PrimitiveExpression(NumberPrimitive.From("4"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression !var1
    /// </summary>
    [Fact]
    public void ParsingNegationReturnsNegationExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.BANG},
            new() {Type = TokenType.ID, Content = "var1"}
        };

        var expectedExpression = new NegationExpression(
            new PrimitiveExpression(IdentifierPrimitive.From("var1"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression -var1
    /// </summary>
    [Fact]
    public void ParsingNegativeReturnsNegativeExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.DASH},
            new() {Type = TokenType.ID, Content = "var1"}
        };

        var expectedExpression = new NegativeExpression(
            new PrimitiveExpression(IdentifierPrimitive.From("var1"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression ++var1
    /// </summary>
    [Fact]
    public void ParsingPrefixAdditionReturnsPrefixAdditionExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.PLUS_PLUS},
            new() {Type = TokenType.ID, Content = "var1"}
        };

        var expectedExpression = new PrefixAdditionExpression(
            new PrimitiveExpression(IdentifierPrimitive.From("var1"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression --var
    /// </summary>
    [Fact]
    public void ParsingPrefixSubtractionReturnsPrefixSubtractionExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.DASH_DASH},
            new() {Type = TokenType.ID, Content = "var1"}
        };

        var expectedExpression = new PrefixSubtractionExpression(
            new PrimitiveExpression(IdentifierPrimitive.From("var1"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    [Fact]
    private void PrefixThrowsExceptionWhenFollowedByNumber()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.DASH_DASH},
            new() {Type = TokenType.NUMBER_CONST, Content = "2", Index = 3, Line = 2},
            new() {Type = TokenType.SEMICOLON}
        };

        AssertFails<Exception>(tokens);
        // var exception = AssertFails<InvalidPrefixExpressionException>(tokens);
        // Assert.Equal("Prefix can only be used with variables (2:3)", exception.EnglishMessage);
        // Assert.Equal("العمليات الحسابية المقدمة يجب أن تستعمل مع المتغيرات فقط (2:3)", exception.ArabicMessage);
    }

    [Fact]
    private void PrefixThrowsExceptionWhenFollowedByField()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.DASH_DASH},
            new() {Type = TokenType.ID, Content = "instance"},
            new() {Type = TokenType.DOT},
            new() {Type = TokenType.ID, Content = "field", Index = 3, Line = 2},
            new() {Type = TokenType.SEMICOLON},
        };

        AssertFails<Exception>(tokens);
        // var exception = AssertFails<InvalidPrefixExpressionException>(tokens);
        // Assert.Equal("Prefix can only be used with variables (2:3)", exception.EnglishMessage);
        // Assert.Equal("العمليات الحسابية المقدمة يجب أن تستعمل مع المتغيرات فقط (2:3)", exception.ArabicMessage);
    }


    /// <summary>
    /// parsing expression var1++
    /// </summary>
    [Fact]
    public void ParsingPostfixAdditionReturnsPostfixAdditionExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.ID, Content = "var1"},
            new() {Type = TokenType.PLUS_PLUS}
        };

        var expectedExpression = new PostfixAdditionExpression(
            new PrimitiveExpression(IdentifierPrimitive.From("var1"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression var--
    /// </summary>
    [Fact]
    public void ParsingPostfixSubtractionReturnsPostfixSubtractionExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.ID, Content = "var1"},
            new() {Type = TokenType.DASH_DASH}
        };

        var expectedExpression = new PostfixSubtractionExpression(
            new PrimitiveExpression(IdentifierPrimitive.From("var1"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    [Fact]
    private void ParsingPostfixThrowsExceptionWhenPrecededByNumber()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.NUMBER_CONST, Content = "2", Index = 3, Line = 2},
            new() {Type = TokenType.PLUS_PLUS},
            new() {Type = TokenType.SEMICOLON}
        };

        AssertFails<Exception>(tokens);
        // var exception = AssertFails<InvalidPostfixExpressionException>(tokens);
        // Assert.Equal("Postfix can only be used with variables (2:3)", exception.EnglishMessage);
        // Assert.Equal("العمليات الحسابية المؤخرة يجب أن تستعمل مع المتغيرات فقط (2:3)", exception.ArabicMessage);
    }

    [Fact]
    private void ParsingPostfixThrowsExceptionWhenPrecededByField()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.ID, Content = "instance"},
            new() {Type = TokenType.DOT},
            new() {Type = TokenType.ID, Content = "field", Index = 3, Line = 2},
            new() {Type = TokenType.PLUS_PLUS},
            new() {Type = TokenType.SEMICOLON}
        };

        AssertFails<Exception>(tokens);
        // var exception = AssertFails<InvalidPostfixExpressionException>(tokens);
        // Assert.Equal("Postfix can only be used with variables (2:3)", exception.EnglishMessage);
        // Assert.Equal("العمليات الحسابية المؤخرة يجب أن تستعمل مع المتغيرات فقط (2:3)", exception.ArabicMessage);
    }

    /// <summary>
    /// parsing expression toNumber(var1)
    /// </summary>
    [Fact]
    public void ParsingToNumberReturnsToNumberExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.NUMBER},
            new() {Type = TokenType.OPEN_PAREN},
            new() {Type = TokenType.ID, Content = "var1"},
            new() {Type = TokenType.CLOSE_PAREN}
        };

        var expectedExpression = new ToNumberExpression(
            new PrimitiveExpression(IdentifierPrimitive.From("var1"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression toBool(var1)
    /// </summary>
    [Fact]
    public void ParsingToBoolReturnsToBoolExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.BOOL},
            new() {Type = TokenType.OPEN_PAREN},
            new() {Type = TokenType.ID, Content = "var1"},
            new() {Type = TokenType.CLOSE_PAREN}
        };

        var expectedExpression = new ToBoolExpression(
            new PrimitiveExpression(IdentifierPrimitive.From("var1"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression toString(var1)
    /// </summary>
    [Fact]
    public void ParsingToStringReturnsToStringExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.STRING},
            new() {Type = TokenType.OPEN_PAREN},
            new() {Type = TokenType.ID, Content = "var1"},
            new() {Type = TokenType.CLOSE_PAREN}
        };

        var expectedExpression = new ToStringExpression(
            new PrimitiveExpression(IdentifierPrimitive.From("var1"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression typeof(var1)
    /// </summary>
    [Fact]
    public void ParsingTypeofReturnsTypeofExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.TYPEOF},
            new() {Type = TokenType.OPEN_PAREN},
            new() {Type = TokenType.ID, Content = "var1"},
            new() {Type = TokenType.CLOSE_PAREN}
        };

        var expectedExpression = new TypeOfExpression(
            new PrimitiveExpression(IdentifierPrimitive.From("var1"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression: (var1)
    /// </summary>
    [Fact]
    public void ParsingGroupReturnsGroupExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.OPEN_PAREN},
            new() {Type = TokenType.ID, Content = "var1"},
            new() {Type = TokenType.CLOSE_PAREN}
        };

        var expectedExpression = new GroupExpression(
            new PrimitiveExpression(IdentifierPrimitive.From("var1"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression: instance.field
    /// </summary>
    [Fact]
    public void ParsingInstanceFieldReturnsInstanceFieldExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.ID, Content = "instance"},
            new() {Type = TokenType.DOT},
            new() {Type = TokenType.ID, Content = "field"}
        };

        var expectedExpression = new InstanceFieldExpression(
            new PrimitiveExpression(IdentifierPrimitive.From("instance")),
            new PrimitiveExpression(IdentifierPrimitive.From("field"))
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    [Fact]
    public void ParsingInstanceFieldThrowsExceptionIfInstanceWasNotIdentifiers()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.NUMBER, Content = "1"},
            new() {Type = TokenType.DOT},
            new() {Type = TokenType.ID, Content = "field"}
        };

        var strategy = new ParsingExpressionStrategy();
        Assert.Throws<Exception>(() => strategy.Parse(tokens, 0));
        // Assert.Throws<MissingTokenException>(() => strategy.Parse(tokens, 0));
    }

    [Fact]
    private void ThrowsExceptionIfFieldTokenWasNotFound()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.ID, Content = "instance"},
            new() {Type = TokenType.DOT},
            new() {Type = TokenType.NUMBER_CONST, Content = "3"},
        };

        AssertFails<Exception>(tokens);
        // AssertFails<MissingExpressionException>(tokens);
    }

    [Fact]
    public void ParsingInstanceFieldThrowsExceptionIfFieldWasNotIdentifier()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.ID, Content = "instance"},
            new() {Type = TokenType.DOT},
            new() {Type = TokenType.NUMBER, Content = "1", Line = 1, Index = 3},
        };

        var strategy = new ParsingExpressionStrategy();
        Assert.NotNull(Record.Exception(() => strategy.Parse(tokens, 0)));
        // var exception = Assert.Throws<MissingExpressionException>(() => strategy.Parse(tokens, 0));
        // Assert.Equal("Expected expression of type ID was not found at line 1:3", exception.EnglishMessage);
        // Assert.Equal("عبارة متوقعة من نوع ID لم توجد على السطر 1:3", exception.ArabicMessage);
    }

    /// <summary>
    /// parsing expression: instance.method()
    /// </summary>
    [Fact]
    public void ParsingInstanceMethodCallReturnsInstanceMethodCallExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.ID, Content = "instance"},
            new() {Type = TokenType.DOT},
            new() {Type = TokenType.ID, Content = "method"},
            new() {Type = TokenType.OPEN_PAREN},
            new() {Type = TokenType.CLOSE_PAREN}
        };

        var expectedExpression = new InstanceMethodCallExpression(
            new PrimitiveExpression(IdentifierPrimitive.From("instance")),
            new PrimitiveExpression(IdentifierPrimitive.From("method")),
            new List<Expression>()
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    [Fact]
    private void ShouldThrowExceptionWhenInstanceMethodCallDoesNotCloseParenthesis()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.ID, Content = "instance"},
            new() {Type = TokenType.DOT},
            new() {Type = TokenType.ID, Content = "method"},
            new() {Type = TokenType.OPEN_PAREN, Line = 1, Index = 3},
        };

        AssertFails<Exception>(tokens);
        // var exception = AssertFails<MissingTokenException>(tokens);
        // Assert.Equal("Expected token of type CLOSE_PAREN was not found at line 1:4", exception.EnglishMessage);
    }


    [Fact]
    private void ShouldThrowExceptionWhenInstanceMethodCallDoesNotCloseParenthesisWhenFollowedByMoreTokens()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.ID, Content = "instance"},
            new() {Type = TokenType.DOT},
            new() {Type = TokenType.ID, Content = "method"},
            new() {Type = TokenType.OPEN_PAREN},
            new() {Type = TokenType.SEMICOLON, Line = 1, Index = 4},
        };

        Assert.NotNull(Record.Exception(() => new ParsingExpressionStrategy().Parse(tokens, 0)));
        // var exception = AssertFails<MissingTokenException>(tokens);
        // Assert.Equal("Expected token of type CLOSE_PAREN was not found at line 1:4", exception.EnglishMessage);
    }

    [Fact]
    private void ShouldThrowExceptionWhenArgumentsOfInstanceMethodCallAreNotSeparatedByCommas()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.ID, Content = "instance"},
            new() {Type = TokenType.DOT},
            new() {Type = TokenType.ID, Content = "method"},
            new() {Type = TokenType.OPEN_PAREN},
            new() {Type = TokenType.ID, Content = "arg1"},
            new() {Type = TokenType.ID, Content = "arg2", Index = 5, Line = 9},
            new() {Type = TokenType.CLOSE_PAREN}
        };

        AssertFails<Exception>(tokens);
        // var exception = AssertFails<MissingTokenException>(tokens);
        // Assert.Equal("Expected token of type CLOSE_PAREN was not found at line 9:5", exception.EnglishMessage);
    }

    /// <summary>
    /// parsing expression: instance.method(arg1, 2)
    /// </summary>
    [Fact]
    public void ParsingInstanceMethodCallWithArgumentsReturnsInstanceMethodCallExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.ID, Content = "instance"},
            new() {Type = TokenType.DOT},
            new() {Type = TokenType.ID, Content = "method"},
            new() {Type = TokenType.OPEN_PAREN},
            new() {Type = TokenType.ID, Content = "arg1"},
            new() {Type = TokenType.COMMA},
            new() {Type = TokenType.NUMBER_CONST, Content = "2"},
            new() {Type = TokenType.CLOSE_PAREN}
        };

        var expectedExpression = new InstanceMethodCallExpression(
            new PrimitiveExpression(IdentifierPrimitive.From("instance")),
            new PrimitiveExpression(IdentifierPrimitive.From("method")),
            new List<Expression>
            {
                new PrimitiveExpression(IdentifierPrimitive.From("arg1")),
                new PrimitiveExpression(NumberPrimitive.From("2"))
            }
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    [Fact]
    public void ParsingMethodCallReturnsMethodCallExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.ID, Content = "method"},
            new() {Type = TokenType.OPEN_PAREN},
            new() {Type = TokenType.CLOSE_PAREN}
        };

        var expectedExpression = new CallExpression(
            new PrimitiveExpression(IdentifierPrimitive.From("method")),
            new List<Expression>()
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression method(arg1, 2)
    /// </summary>
    [Fact]
    public void ParsingMethodCallWithArgumentsReturnsMethodCallExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.ID, Content = "method"},
            new() {Type = TokenType.OPEN_PAREN},
            new() {Type = TokenType.ID, Content = "arg1"},
            new() {Type = TokenType.COMMA},
            new() {Type = TokenType.NUMBER_CONST, Content = "2"},
            new() {Type = TokenType.CLOSE_PAREN}
        };

        var expectedExpression = new CallExpression(
            new PrimitiveExpression(IdentifierPrimitive.From("method")),
            new List<Expression>
            {
                new PrimitiveExpression(IdentifierPrimitive.From("arg1")),
                new PrimitiveExpression(NumberPrimitive.From("2"))
            }
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression: new class()
    /// </summary>
    [Fact]
    private void ParsingInstantiationReturnsInstantiationExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.NEW},
            new() {Type = TokenType.ID, Content = "class"},
            new() {Type = TokenType.OPEN_PAREN},
            new() {Type = TokenType.CLOSE_PAREN}
        };

        var expectedExpression = new InstantiationExpression(
            new PrimitiveExpression(IdentifierPrimitive.From("class")),
            new List<Expression>()
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    [Fact]
    private void ParsingInstantiationThrowsExceptionIfTargetClassWasNotIdentifierPrimitive()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.NEW},
            new() {Type = TokenType.STRING_CONST, Content = "class"},
            new() {Type = TokenType.OPEN_PAREN, Index = 3, Line = 4},
            new() {Type = TokenType.CLOSE_PAREN}
        };

        var strategy = new ParsingExpressionStrategy();
        Assert.Throws<Exception>(() => strategy.Parse(tokens, 0));
        // var exception = Assert.Throws<MissingExpressionException>(() => strategy.Parse(tokens, 0));
        // Assert.Equal("Expected expression of type ID was not found at line 4:3", exception.EnglishMessage);
        // Assert.Equal("عبارة متوقعة من نوع ID لم توجد على السطر 4:3", exception.ArabicMessage);
    }

    /// <summary>
    /// parsing expression: new class(arg1, 2)
    /// </summary>
    [Fact]
    private void ParsingInstantiationWithArgumentsReturnsInstantiationExpression()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.NEW},
            new() {Type = TokenType.ID, Content = "class"},
            new() {Type = TokenType.OPEN_PAREN},
            new() {Type = TokenType.ID, Content = "arg1"},
            new() {Type = TokenType.COMMA},
            new() {Type = TokenType.NUMBER_CONST, Content = "2"},
            new() {Type = TokenType.CLOSE_PAREN}
        };

        var expectedExpression = new InstantiationExpression(
            new PrimitiveExpression(IdentifierPrimitive.From("class")),
            new List<Expression>
            {
                new PrimitiveExpression(IdentifierPrimitive.From("arg1")),
                new PrimitiveExpression(NumberPrimitive.From("2")),
            }
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parses expression: var1 < 3 || !var2 && typeof(var3) == "string"
    /// </summary>
    [Fact]
    private void ParsingCompoundExpression()
    {
        AssertionOptions.FormattingOptions.MaxDepth = 100;


        var tokens = new List<Token>()
        {
            new() {Type = TokenType.ID, Content = "var1"},
            new() {Type = TokenType.LESS_THAN},
            new() {Type = TokenType.NUMBER_CONST, Content = "3"},
            new() {Type = TokenType.OR},
            new() {Type = TokenType.BANG},
            new() {Type = TokenType.ID, Content = "var2"},
            new() {Type = TokenType.AND},
            new() {Type = TokenType.TYPEOF},
            new() {Type = TokenType.OPEN_PAREN},
            new() {Type = TokenType.ID, Content = "var3"},
            new() {Type = TokenType.CLOSE_PAREN},
            new() {Type = TokenType.EQUAL_EQUAL},
            new() {Type = TokenType.STRING_CONST, Content = "string"}
        };

        var expectedExpression = new OrOperationExpression(
            new LessCheckExpression(
                new PrimitiveExpression(IdentifierPrimitive.From("var1")),
                new PrimitiveExpression(NumberPrimitive.From("3"))
            ),
            new AndOperationExpression(
                new NegationExpression(
                    new PrimitiveExpression(IdentifierPrimitive.From("var2"))
                ),
                new EqualityCheckExpression(
                    new TypeOfExpression(
                        new PrimitiveExpression(IdentifierPrimitive.From("var3"))
                    ),
                    new PrimitiveExpression(StringPrimitive.From("string"))
                )
            )
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression: var1 && (!var2 || var3) || var4 < var5++
    /// </summary>
    [Fact]
    private void ParsingCompoundExpression2()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.ID, Content = "var1"},
            new() {Type = TokenType.AND},
            new() {Type = TokenType.OPEN_PAREN},
            new() {Type = TokenType.BANG},
            new() {Type = TokenType.ID, Content = "var2"},
            new() {Type = TokenType.OR},
            new() {Type = TokenType.ID, Content = "var3"},
            new() {Type = TokenType.CLOSE_PAREN},
            new() {Type = TokenType.OR},
            new() {Type = TokenType.ID, Content = "var4"},
            new() {Type = TokenType.LESS_THAN},
            new() {Type = TokenType.ID, Content = "var5"},
            new() {Type = TokenType.PLUS_PLUS},
        };

        var expectedExpression = new OrOperationExpression(
            new AndOperationExpression(
                new PrimitiveExpression(IdentifierPrimitive.From("var1")),
                new GroupExpression(
                    new OrOperationExpression(
                        new NegationExpression(
                            new PrimitiveExpression(IdentifierPrimitive.From("var2"))
                        ),
                        new PrimitiveExpression(IdentifierPrimitive.From("var3"))
                    )
                )
            ),
            new LessCheckExpression(
                new PrimitiveExpression(IdentifierPrimitive.From("var4")),
                new PostfixAdditionExpression(
                    new PrimitiveExpression(IdentifierPrimitive.From("var5"))
                )
            )
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression: 3 * var1 + true || (!var2 + ++var3) && 4 <= -(var4 % numberOf("2"))
    /// </summary>
    [Fact]
    private void ParseCompoundExpression3()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.NUMBER_CONST, Content = "3"},
            new() {Type = TokenType.STAR},
            new() {Type = TokenType.ID, Content = "var1"},
            new() {Type = TokenType.PLUS},
            new() {Type = TokenType.TRUE},
            new() {Type = TokenType.OR},
            new() {Type = TokenType.OPEN_PAREN},
            new() {Type = TokenType.BANG},
            new() {Type = TokenType.ID, Content = "var2"},
            new() {Type = TokenType.PLUS},
            new() {Type = TokenType.PLUS_PLUS},
            new() {Type = TokenType.ID, Content = "var3"},
            new() {Type = TokenType.CLOSE_PAREN},
            new() {Type = TokenType.AND},
            new() {Type = TokenType.NUMBER_CONST, Content = "4"},
            new() {Type = TokenType.LESS_EQUAL},
            new() {Type = TokenType.DASH},
            new() {Type = TokenType.OPEN_PAREN},
            new() {Type = TokenType.ID, Content = "var4"},
            new() {Type = TokenType.MODULO},
            new() {Type = TokenType.NUMBER},
            new() {Type = TokenType.OPEN_PAREN},
            new() {Type = TokenType.STRING_CONST, Content = "2"},
            new() {Type = TokenType.CLOSE_PAREN},
            new() {Type = TokenType.CLOSE_PAREN},
        };

        var expectedExpression = new OrOperationExpression(
            new AdditionExpression(
                new MultiplicationExpression(
                    new PrimitiveExpression(NumberPrimitive.From("3")),
                    new PrimitiveExpression(IdentifierPrimitive.From("var1"))
                ),
                new PrimitiveExpression(BoolPrimitive.True())
            ),
            new AndOperationExpression(
                new GroupExpression(
                    new AdditionExpression(
                        new NegationExpression(
                            new PrimitiveExpression(IdentifierPrimitive.From("var2"))
                        ),
                        new PrefixAdditionExpression(
                            new PrimitiveExpression(IdentifierPrimitive.From("var3"))
                        )
                    )
                ),
                new LessOrEqualCheckExpression(
                    new PrimitiveExpression(NumberPrimitive.From("4")),
                    new NegationExpression(
                        new GroupExpression(
                            new ModuloExpression(
                                new PrimitiveExpression(IdentifierPrimitive.From("var4")),
                                new ToNumberExpression(
                                    new PrimitiveExpression(StringPrimitive.From("2"))
                                )
                            )
                        )
                    )
                )
            )
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    /// <summary>
    /// parsing expression: 3 * var1 + true || (!var2 + ++var3) && 4 <= -(var4 % numberOf("2"))
    /// </summary>
    [Fact]
    private void IgnoresWhiteSpaces()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.NUMBER_CONST, Content = "3"},
            new() {Type = TokenType.WHITE_SPACE},
            new() {Type = TokenType.STAR},
            new() {Type = TokenType.ID, Content = "var1"},
            new() {Type = TokenType.WHITE_SPACE},
            new() {Type = TokenType.PLUS},
            new() {Type = TokenType.WHITE_SPACE},
            new() {Type = TokenType.TRUE},
            new() {Type = TokenType.WHITE_SPACE},
            new() {Type = TokenType.OR},
            new() {Type = TokenType.WHITE_SPACE},
            new() {Type = TokenType.OPEN_PAREN},
            new() {Type = TokenType.BANG},
            new() {Type = TokenType.ID, Content = "var2"},
            new() {Type = TokenType.WHITE_SPACE},
            new() {Type = TokenType.PLUS},
            new() {Type = TokenType.WHITE_SPACE},
            new() {Type = TokenType.PLUS_PLUS},
            new() {Type = TokenType.ID, Content = "var3"},
            new() {Type = TokenType.CLOSE_PAREN},
            new() {Type = TokenType.WHITE_SPACE},
            new() {Type = TokenType.AND},
            new() {Type = TokenType.WHITE_SPACE},
            new() {Type = TokenType.NUMBER_CONST, Content = "4"},
            new() {Type = TokenType.WHITE_SPACE},
            new() {Type = TokenType.LESS_EQUAL},
            new() {Type = TokenType.WHITE_SPACE},
            new() {Type = TokenType.DASH},
            new() {Type = TokenType.OPEN_PAREN},
            new() {Type = TokenType.ID, Content = "var4"},
            new() {Type = TokenType.WHITE_SPACE},
            new() {Type = TokenType.MODULO},
            new() {Type = TokenType.WHITE_SPACE},
            new() {Type = TokenType.NUMBER},
            new() {Type = TokenType.OPEN_PAREN},
            new() {Type = TokenType.STRING_CONST, Content = "2"},
            new() {Type = TokenType.CLOSE_PAREN},
            new() {Type = TokenType.CLOSE_PAREN}
        };

        var expectedExpression = new OrOperationExpression(
            new AdditionExpression(
                new MultiplicationExpression(
                    new PrimitiveExpression(NumberPrimitive.From("3")),
                    new PrimitiveExpression(IdentifierPrimitive.From("var1"))
                ),
                new PrimitiveExpression(BoolPrimitive.True())
            ),
            new AndOperationExpression(
                new GroupExpression(
                    new AdditionExpression(
                        new NegationExpression(
                            new PrimitiveExpression(IdentifierPrimitive.From("var2"))
                        ),
                        new PrefixAdditionExpression(
                            new PrimitiveExpression(IdentifierPrimitive.From("var3"))
                        )
                    )
                ),
                new LessOrEqualCheckExpression(
                    new PrimitiveExpression(NumberPrimitive.From("4")),
                    new NegationExpression(
                        new GroupExpression(
                            new ModuloExpression(
                                new PrimitiveExpression(IdentifierPrimitive.From("var4")),
                                new ToNumberExpression(
                                    new PrimitiveExpression(StringPrimitive.From("2"))
                                )
                            )
                        )
                    )
                )
            )
        );

        ParseAndAssertResult(tokens, expectedExpression);
    }

    private static void ParseAndAssertResult(List<Token> tokens, Expression expectedExpression)
    {
        var strategy = new ParsingExpressionStrategy();
        var expression = strategy.Parse(tokens, 0);

        expectedExpression
            .Should()
            .BeEquivalentTo(expression, options => options.RespectingRuntimeTypes());
    }

    private static T AssertFails<T>(List<Token> tokens) where T : Exception
    {
        var strategy = new ParsingExpressionStrategy();
        return Assert.Throws<T>(() => strategy.Parse(tokens, 0));
    }
}