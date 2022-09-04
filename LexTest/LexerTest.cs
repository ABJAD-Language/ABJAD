using System;
using System.Collections.Generic;
using LexEngine;
using Moq;
using Xunit;

namespace LexTest;

public class LexerTest
{
    private readonly Mock<StringUtils> stringUtils;
    
    public LexerTest()
    {
        stringUtils = new Mock<StringUtils>();
    }

    [Fact]
    private void InitializingLexerCallsStringUtilsIgnoreCaseSensitivity()
    {
        new Lexer("code", stringUtils.Object);
        stringUtils.Verify(mock => mock.IgnoreCaseSensitivity("code"));
    }

    [Fact]
    private void ThrowsNullExceptionWhenCodeIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new Lexer(null, stringUtils.Object));
    }

    [Fact]
    private void ReturnsEmptyListForEmptyCode()
    {
        var lexer = new Lexer("", stringUtils.Object);
        Assert.Empty(lexer.Lex());
    }

    [Fact]
    private void ReturnsListOfOneElementWhenCodeContainsSingleCharacter()
    {
        var lexer = new Lexer("(", stringUtils.Object);
        Assert.Single(lexer.Lex());
    }
}