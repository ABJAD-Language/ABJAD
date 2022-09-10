using LexEngine.Characters;
using LexEngine.Tokens;
using Xunit;

namespace LexEngine.Test.Tokens;

public class TokenScannerTest
{
    [Fact]
    private void ScansRightParenthesisCorrectly()
    {
        var token = TokenScanner.ScanToken("(", 10, 2, 3, CharacterType.RIGHT_PAREN);
        Assert.Equal(10, token.StartIndex);
        Assert.Equal(10, token.EndIndex);
        Assert.Equal(3, token.StartLine);
        Assert.Equal(2, token.StartLineIndex);
        Assert.Equal(2, token.EndLineIndex);
        Assert.Equal("(", token.Label);
        Assert.Equal(TokenType.OPEN_PAREN, token.Type);
    }

    [Fact]
    private void ScansLeftParenthesisCorrectly()
    {
        var token = TokenScanner.ScanToken(")", 1, 1, 1, CharacterType.LEFT_PAREN);
        Assert.Equal(1, token.StartIndex);
        Assert.Equal(1, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(1, token.StartLineIndex);
        Assert.Equal(1, token.EndLineIndex);
        Assert.Equal(")", token.Label);
        Assert.Equal(TokenType.CLOSE_PAREN, token.Type);
    }

    [Fact]
    private void ScansRightBraceCorrectly()
    {
        var token = TokenScanner.ScanToken("{", 10, 5, 3, CharacterType.RIGHT_BRACE);
        Assert.Equal(10, token.StartIndex);
        Assert.Equal(10, token.EndIndex);
        Assert.Equal(3, token.StartLine);
        Assert.Equal(5, token.StartLineIndex);
        Assert.Equal(5, token.EndLineIndex);
        Assert.Equal("{", token.Label);
        Assert.Equal(TokenType.OPEN_BRACE, token.Type);
    }
    
    [Fact]
    private void ScansLeftBraceCorrectly()
    {
        var token = TokenScanner.ScanToken("}", 10, 5, 3, CharacterType.LEFT_BRACE);
        Assert.Equal(10, token.StartIndex);
        Assert.Equal(10, token.EndIndex);
        Assert.Equal(3, token.StartLine);
        Assert.Equal(5, token.StartLineIndex);
        Assert.Equal(5, token.EndLineIndex);
        Assert.Equal("}", token.Label);
        Assert.Equal(TokenType.CLOSE_BRACE, token.Type);
    }
    
    [Fact]
    private void ScansSemicolonCorrectly()
    {
        var token = TokenScanner.ScanToken("؛", 10, 5, 3, CharacterType.SEMICOLON);
        Assert.Equal(10, token.StartIndex);
        Assert.Equal(10, token.EndIndex);
        Assert.Equal(3, token.StartLine);
        Assert.Equal(5, token.StartLineIndex);
        Assert.Equal(5, token.EndLineIndex);
        Assert.Equal("؛", token.Label);
        Assert.Equal(TokenType.SEMICOLON, token.Type);
    }
    
    [Fact]
    private void ScansCommaCorrectly()
    {
        var token = TokenScanner.ScanToken("،", 10, 5, 3, CharacterType.COMMA);
        Assert.Equal(10, token.StartIndex);
        Assert.Equal(10, token.EndIndex);
        Assert.Equal(3, token.StartLine);
        Assert.Equal(5, token.StartLineIndex);
        Assert.Equal(5, token.EndLineIndex);
        Assert.Equal("،", token.Label);
        Assert.Equal(TokenType.COMMA, token.Type);
    }
    
    [Fact]
    private void ScansDotCorrectly()
    {
        var token = TokenScanner.ScanToken(".", 10, 5, 3, CharacterType.DOT);
        Assert.Equal(10, token.StartIndex);
        Assert.Equal(10, token.EndIndex);
        Assert.Equal(3, token.StartLine);
        Assert.Equal(5, token.StartLineIndex);
        Assert.Equal(5, token.EndLineIndex);
        Assert.Equal(".", token.Label);
        Assert.Equal(TokenType.DOT, token.Type);
    }
    
    [Fact]
    private void ScansModuloCorrectly()
    {
        var token = TokenScanner.ScanToken("%", 10, 5, 3, CharacterType.PERCENTAGE);
        Assert.Equal(10, token.StartIndex);
        Assert.Equal(10, token.EndIndex);
        Assert.Equal(3, token.StartLine);
        Assert.Equal(5, token.StartLineIndex);
        Assert.Equal(5, token.EndLineIndex);
        Assert.Equal("%", token.Label);
        Assert.Equal(TokenType.MODULO, token.Type);
    }
    
    [Fact]
    private void ScansMinusCorrectly()
    {
        var token = TokenScanner.ScanToken("-", 1, 1, 1, CharacterType.DASH);
        Assert.Equal(1, token.StartIndex);
        Assert.Equal(1, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(1, token.StartLineIndex);
        Assert.Equal(1, token.EndLineIndex);
        Assert.Equal("-", token.Label);
        Assert.Equal(TokenType.MINUS, token.Type);
    }
    
    [Fact]
    private void ScansPlusCorrectly()
    {
        var token = TokenScanner.ScanToken("+", 1, 1, 1, CharacterType.PLUS);
        Assert.Equal(1, token.StartIndex);
        Assert.Equal(1, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(1, token.StartLineIndex);
        Assert.Equal(1, token.EndLineIndex);
        Assert.Equal("+", token.Label);
        Assert.Equal(TokenType.PLUS, token.Type);
    }
    
    [Fact]
    private void ScansTimesSignCorrectly()
    {
        var token = TokenScanner.ScanToken("*", 1, 1, 1, CharacterType.STAR);
        Assert.Equal(1, token.StartIndex);
        Assert.Equal(1, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(1, token.StartLineIndex);
        Assert.Equal(1, token.EndLineIndex);
        Assert.Equal("*", token.Label);
        Assert.Equal(TokenType.TIMES, token.Type);
    }
    
    [Fact]
    private void ScansDivideBySignCorrectly()
    {
        var token = TokenScanner.ScanToken("\\", 1, 1, 1, CharacterType.SLASH);
        Assert.Equal(1, token.StartIndex);
        Assert.Equal(1, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(1, token.StartLineIndex);
        Assert.Equal(1, token.EndLineIndex);
        Assert.Equal("\\", token.Label);
        Assert.Equal(TokenType.DIVIDED_BY, token.Type);
    }
    
    [Fact]
    private void ScansEqualSignCorrectlyAtEndOfCode()
    {
        var token = TokenScanner.ScanToken("=", 1, 1, 1, CharacterType.EQUAL);
        Assert.Equal(1, token.StartIndex);
        Assert.Equal(1, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(1, token.StartLineIndex);
        Assert.Equal(1, token.EndLineIndex);
        Assert.Equal("=", token.Label);
        Assert.Equal(TokenType.EQUAL, token.Type);
    }
    
    [Fact]
    private void ScansEqualSignCorrectly()
    {
        var token = TokenScanner.ScanToken("=1", 1, 1, 1, CharacterType.EQUAL);
        Assert.Equal(1, token.StartIndex);
        Assert.Equal(1, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(1, token.StartLineIndex);
        Assert.Equal(1, token.EndLineIndex);
        Assert.Equal("=", token.Label);
        Assert.Equal(TokenType.EQUAL, token.Type);
    }
    
    [Fact]
    private void ScansEqualEqualSignCorrectly()
    {
        var token = TokenScanner.ScanToken("==", 1, 1, 1, CharacterType.EQUAL);
        Assert.Equal(1, token.StartIndex);
        Assert.Equal(2, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(1, token.StartLineIndex);
        Assert.Equal(2, token.EndLineIndex);
        Assert.Equal("==", token.Label);
        Assert.Equal(TokenType.EQUAL_EQUAL, token.Type);
    }
    
    [Fact]
    private void ScansBangSignCorrectlyWhenEndOfCode()
    {
        var token = TokenScanner.ScanToken("!", 1, 1, 1, CharacterType.EXCLAMATION_MARK);
        Assert.Equal(1, token.StartIndex);
        Assert.Equal(1, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(1, token.StartLineIndex);
        Assert.Equal(1, token.EndLineIndex);
        Assert.Equal("!", token.Label);
        Assert.Equal(TokenType.BANG, token.Type);
    }
    
    [Fact]
    private void ScansBangSignCorrectly()
    {
        var token = TokenScanner.ScanToken("!true", 1, 1, 1, CharacterType.EXCLAMATION_MARK);
        Assert.Equal(1, token.StartIndex);
        Assert.Equal(1, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(1, token.StartLineIndex);
        Assert.Equal(1, token.EndLineIndex);
        Assert.Equal("!", token.Label);
        Assert.Equal(TokenType.BANG, token.Type);
    }
    
    [Fact]
    private void ScansBangEqualSignCorrectly()
    {
        var token = TokenScanner.ScanToken("!=", 1, 1, 1, CharacterType.EXCLAMATION_MARK);
        Assert.Equal(1, token.StartIndex);
        Assert.Equal(2, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(1, token.StartLineIndex);
        Assert.Equal(2, token.EndLineIndex);
        Assert.Equal("!=", token.Label);
        Assert.Equal(TokenType.BANG_EQUAL, token.Type);
    }
    
    [Fact]
    private void ScansLessThanSignCorrectlyWhenEndOfLine()
    {
        var token = TokenScanner.ScanToken("أ<", 2, 2, 1, CharacterType.RIGHT_SINGLE_ANGLE);
        Assert.Equal(2, token.StartIndex);
        Assert.Equal(2, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(2, token.StartLineIndex);
        Assert.Equal(2, token.EndLineIndex);
        Assert.Equal("<", token.Label);
        Assert.Equal(TokenType.LESS_THAN, token.Type);
    }
    
    [Fact]
    private void ScansLessThanSignCorrectlyWhenFollowedByCode()
    {
        var token = TokenScanner.ScanToken("أ<1", 2, 2, 1, CharacterType.RIGHT_SINGLE_ANGLE);
        Assert.Equal(2, token.StartIndex);
        Assert.Equal(2, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(2, token.StartLineIndex);
        Assert.Equal(2, token.EndLineIndex);
        Assert.Equal("<", token.Label);
        Assert.Equal(TokenType.LESS_THAN, token.Type);
    }
    
    [Fact]
    private void ScansLessThanOrEqualSignCorrectlyWhenEndOfLine()
    {
        var token = TokenScanner.ScanToken("ا<=2", 2, 2, 1, CharacterType.RIGHT_SINGLE_ANGLE);
        Assert.Equal(2, token.StartIndex);
        Assert.Equal(3, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(2, token.StartLineIndex);
        Assert.Equal(3, token.EndLineIndex);
        Assert.Equal("<=", token.Label);
        Assert.Equal(TokenType.LESS_EQUAL, token.Type);
    }
    
    [Fact]
    private void ScansGreaterThanSignCorrectlyWhenEndOfLine()
    {
        var token = TokenScanner.ScanToken("أ>", 2, 2, 1, CharacterType.LEFT_SINGLE_ANGLE);
        Assert.Equal(2, token.StartIndex);
        Assert.Equal(2, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(2, token.StartLineIndex);
        Assert.Equal(2, token.EndLineIndex);
        Assert.Equal(">", token.Label);
        Assert.Equal(TokenType.GREATER_THAN, token.Type);
    }
    
    [Fact]
    private void ScansGreaterThanSignCorrectlyWhenFollowedByCode()
    {
        var token = TokenScanner.ScanToken("أ>1", 2, 2, 1, CharacterType.LEFT_SINGLE_ANGLE);
        Assert.Equal(2, token.StartIndex);
        Assert.Equal(2, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(2, token.StartLineIndex);
        Assert.Equal(2, token.EndLineIndex);
        Assert.Equal(">", token.Label);
        Assert.Equal(TokenType.GREATER_THAN, token.Type);
    }
    
    [Fact]
    private void ScansGreaterThanOrEqualSignCorrectly()
    {
        var token = TokenScanner.ScanToken("أ>=1", 2, 2, 1, CharacterType.LEFT_SINGLE_ANGLE);
        Assert.Equal(2, token.StartIndex);
        Assert.Equal(3, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(2, token.StartLineIndex);
        Assert.Equal(3, token.EndLineIndex);
        Assert.Equal(">=", token.Label);
        Assert.Equal(TokenType.GREATER_EQUAL, token.Type);
    }

    [Fact]
    private void ThrowsExceptionWhenAmpersandExistsAlone()
    {
        var exception = Assert.Throws<MissingTokenException>(() => TokenScanner.ScanToken("صحيح &", 22, 6, 2, CharacterType.AMPERSAND));
        Assert.Equal(6, exception.Index);
        Assert.Equal(2, exception.Line);
        Assert.Equal("&", exception.Token);
        Assert.Equal("Missing Token: Expected '&' at line 2 : 6", exception.EnglishMessage);
        Assert.Equal("رمز مفقود: الرمز '&' لم يوجد على السطر 2 : 6", exception.ArabicMessage);
    }

    [Fact]
    private void ScansAndTokenCorrectlyAtEndOfCode()
    {
        var token = TokenScanner.ScanToken("\n\nصحيح &&", 8, 6, 3, CharacterType.AMPERSAND);
        Assert.Equal(8, token.StartIndex);
        Assert.Equal(9, token.EndIndex);
        Assert.Equal(3, token.StartLine);
        Assert.Equal(6, token.StartLineIndex);
        Assert.Equal(7, token.EndLineIndex);
        Assert.Equal("&&", token.Label);
        Assert.Equal(TokenType.AND, token.Type);
    }

    [Fact]
    private void ScansAndTokenCorrectlyWhenCodeFollows()
    {
        var token = TokenScanner.ScanToken("صحيح && خطأ", 6, 6, 1, CharacterType.AMPERSAND);
        Assert.Equal(6, token.StartIndex);
        Assert.Equal(7, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(6, token.StartLineIndex);
        Assert.Equal(7, token.EndIndex);
        Assert.Equal("&&", token.Label);
        Assert.Equal(TokenType.AND, token.Type);
    }

    [Fact]
    private void ThrowsExceptionWhenVerticalBarExistsAlone()
    {
        var exception = Assert.Throws<MissingTokenException>(() => TokenScanner.ScanToken("صحيح |", 30, 6, 3, CharacterType.VERTICAL_BAR));
        Assert.Equal(6, exception.Index);
        Assert.Equal(3, exception.Line);
        Assert.Equal("|", exception.Token);
        Assert.Equal("Missing Token: Expected '|' at line 3 : 6", exception.EnglishMessage);
        Assert.Equal("رمز مفقود: الرمز '|' لم يوجد على السطر 3 : 6", exception.ArabicMessage);
    }
    
    [Fact]
    private void ScansOrTokenCorrectlyAtEndOfCode()
    {
        var token = TokenScanner.ScanToken("صحيح ||", 6, 6, 1, CharacterType.VERTICAL_BAR);
        Assert.Equal(6, token.StartIndex);
        Assert.Equal(7, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(6, token.StartLineIndex);
        Assert.Equal(7, token.EndLineIndex);
        Assert.Equal("||", token.Label);
        Assert.Equal(TokenType.OR, token.Type);
    }

    [Fact]
    private void ScansOrTokenCorrectlyWhenCodeFollows()
    {
        var token = TokenScanner.ScanToken("صحيح || خطأ", 6, 6, 1, CharacterType.VERTICAL_BAR);
        Assert.Equal(6, token.StartIndex);
        Assert.Equal(7, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(6, token.StartLineIndex);
        Assert.Equal(7, token.EndLineIndex);
        Assert.Equal("||", token.Label);
        Assert.Equal(TokenType.OR, token.Type);
    }
    
    [Fact]
    private void ScansCommentCorrectlyWhenEndOfCode()
    {
        var token = TokenScanner.ScanToken("متغير ا = 4؛#", 13, 13, 1, CharacterType.HASH);
        Assert.Equal(13, token.StartIndex);
        Assert.Equal(13, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal("", token.Label);
        Assert.Equal(13, token.StartLineIndex);
        Assert.Equal(TokenType.COMMENT, token.Type);
    }
    
    [Fact]
    private void ScansCommentCorrectlyWhenLastLineOfCode()
    {
        var token = TokenScanner.ScanToken("متغير ا = 4؛# هذا تعليق", 13, 13, 1, CharacterType.HASH);
        Assert.Equal(13, token.StartIndex);
        Assert.Equal(23, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(" هذا تعليق", token.Label);
        Assert.Equal(13, token.StartLineIndex);
        Assert.Equal(TokenType.COMMENT, token.Type);
    }
    
    [Fact]
    private void ScansCommentCorrectlyWhenFollowedByNewLine()
    {
        var token = TokenScanner.ScanToken("متغير ا = 4؛# هذا تعليق\n", 13, 13, 1, CharacterType.HASH);
        Assert.Equal(13, token.StartIndex);
        Assert.Equal(23, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(" هذا تعليق", token.Label);
        Assert.Equal(13, token.StartLineIndex);
        Assert.Equal(TokenType.COMMENT, token.Type);
    }
    
    [Fact]
    private void ScansCommentCorrectlyWhenFollowedByNewLineAndCode()
    {
        var token = TokenScanner.ScanToken("متغير ا = 4؛# هذا تعليق\nمتغير ج = عدم؛", 13, 13, 1, CharacterType.HASH);
        Assert.Equal(13, token.StartIndex);
        Assert.Equal(23, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(" هذا تعليق", token.Label);
        Assert.Equal(13, token.StartLineIndex);
        Assert.Equal(TokenType.COMMENT, token.Type);
    }
    
    [Fact]
    private void ScansWhiteSpaceCorrectlyWhenEndOfCode()
    {
        var token = TokenScanner.ScanToken("متغير ا = 4؛ ", 13, 13, 1, CharacterType.WHITE_SPACE);
        Assert.Equal(13, token.StartIndex);
        Assert.Equal(13, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(13, token.StartLineIndex);
        Assert.Equal(" ", token.Label);
        Assert.Equal(TokenType.WHITE_SPACE, token.Type);
    }
    
    [Fact]
    private void ScansWhiteSpaceCorrectlyWhenFollowedByCode()
    {
        var token = TokenScanner.ScanToken("متغير ا = 4؛ متغير ا = 4؛", 13, 13, 1, CharacterType.WHITE_SPACE);
        Assert.Equal(13, token.StartIndex);
        Assert.Equal(13, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(13, token.StartLineIndex);
        Assert.Equal(" ", token.Label);
        Assert.Equal(TokenType.WHITE_SPACE, token.Type);
    }
    
    [Fact]
    private void ScansWhiteSpacesCorrectly()
    {
        var token = TokenScanner.ScanToken("متغير ا = 4؛  متغير ا = 4؛", 13, 13, 1, CharacterType.WHITE_SPACE);
        Assert.Equal(13, token.StartIndex);
        Assert.Equal(14, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(13, token.StartLineIndex);
        Assert.Equal("  ", token.Label);
        Assert.Equal(TokenType.WHITE_SPACE, token.Type);
    }
    
    [Fact]
    private void ScansTabsAsWhiteSpaces()
    {
        var token = TokenScanner.ScanToken("متغير ا = 4؛\r متغير ا = 4؛", 13, 13, 1, CharacterType.WHITE_SPACE);
        Assert.Equal(13, token.StartIndex);
        Assert.Equal(14, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(13, token.StartLineIndex);
        Assert.Equal("\r ", token.Label);
        Assert.Equal(TokenType.WHITE_SPACE, token.Type);
    }
    
    [Fact]
    private void ScansWhiteSpaceCorrectlyWhenIsNewlines()
    {
        var token = TokenScanner.ScanToken("متغير ا = 4؛\n\nمتغير ا = 4؛", 13, 13, 1,CharacterType.WHITE_SPACE);
        Assert.Equal(13, token.StartIndex);
        Assert.Equal(14, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(3, token.EndLine);
        Assert.Equal(13, token.StartLineIndex);
        Assert.Equal(1, token.EndLineIndex);
        Assert.Equal("\n\n", token.Label);
        Assert.Equal(TokenType.WHITE_SPACE, token.Type);
    }
    
    [Fact]
    private void ScansNewLineCorrectly()
    {
        var token = TokenScanner.ScanToken("\n", 1, 1, 1, CharacterType.WHITE_SPACE);
        Assert.Equal(1, token.StartIndex);
        Assert.Equal(1, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(2, token.EndLine);
        Assert.Equal(1, token.StartLineIndex);
        Assert.Equal(1, token.EndLineIndex);
        Assert.Equal("\n", token.Label);
        Assert.Equal(TokenType.WHITE_SPACE, token.Type);
    }
    
    [Fact]
    private void ScansWhiteSpaceCorrectlyWhenIsThreeNewlines()
    {
        var token = TokenScanner.ScanToken("متغير ا = 4؛\n\n\nمتغير ا = 4؛", 13, 13, 1, CharacterType.WHITE_SPACE);
        Assert.Equal(13, token.StartIndex);
        Assert.Equal(15, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(4, token.EndLine);
        Assert.Equal(13, token.StartLineIndex);
        Assert.Equal("\n\n\n", token.Label);
        Assert.Equal(TokenType.WHITE_SPACE, token.Type);
    }
    
    [Fact]
    private void ScansStringLiteralCorrectlyWhenEndOfCode()
    {
        var token = TokenScanner.ScanToken("متغير ع = \"مرحبا بالعالم\"", 11, 11, 1, CharacterType.DOUBLE_QUOTE);
        Assert.Equal(11, token.StartIndex);
        Assert.Equal(25, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(11, token.StartLineIndex);
        Assert.Equal(25, token.EndLineIndex);
        Assert.Equal("مرحبا بالعالم", token.Label);
        Assert.Equal(TokenType.STRING_CONST, token.Type);
    }
    
    [Fact]
    private void ScansStringLiteralCorrectlyWhenFollowedByCode()
    {
        var token = TokenScanner.ScanToken("متغير ع = \"مرحبا بالعالم\"؛", 11, 11, 1, CharacterType.DOUBLE_QUOTE);
        Assert.Equal(11, token.StartIndex);
        Assert.Equal(25, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal("مرحبا بالعالم", token.Label);
        Assert.Equal(11, token.StartLineIndex);
        Assert.Equal(25, token.EndLineIndex);
        Assert.Equal(TokenType.STRING_CONST, token.Type);
    }
    
    [Fact]
    private void ScansStringLiteralCorrectlyWhenEmpty()
    {
        var token = TokenScanner.ScanToken("متغير ع = \"\"؛", 11, 11, 1, CharacterType.DOUBLE_QUOTE);
        Assert.Equal(11, token.StartIndex);
        Assert.Equal(12, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal("", token.Label);
        Assert.Equal(11, token.StartLineIndex);
        Assert.Equal(12, token.EndLineIndex);
        Assert.Equal(TokenType.STRING_CONST, token.Type);
    }
    
    [Fact]
    private void ThrowsExceptionWhenNewLineWasFoundInsideString()
    {
        var exception = Assert.Throws<MissingTokenException>(() => TokenScanner.ScanToken("متغير ع = \"\n\"؛", 11, 11, 1, CharacterType.DOUBLE_QUOTE));
        Assert.Equal(12, exception.Index);
        Assert.Equal(1, exception.Line);
        Assert.Equal("\"", exception.Token);
        Assert.Equal("Missing Token: Expected '\"' at line 1 : 12", exception.EnglishMessage);
        Assert.Equal("رمز مفقود: الرمز '\"' لم يوجد على السطر 1 : 12", exception.ArabicMessage);
    }
    
    [Fact]
    private void ThrowsExceptionWhenEndingQuotationIsNotFound()
    {
        var exception = Assert.Throws<MissingTokenException>(() => TokenScanner.ScanToken("متغير ع = \"؛", 11, 11, 1, CharacterType.DOUBLE_QUOTE));
        Assert.Equal(12, exception.Index);
        Assert.Equal(1, exception.Line);
        Assert.Equal("\"", exception.Token);
        Assert.Equal("Missing Token: Expected '\"' at line 1 : 12", exception.EnglishMessage);
        Assert.Equal("رمز مفقود: الرمز '\"' لم يوجد على السطر 1 : 12", exception.ArabicMessage);
    }
    
    [Fact]
    private void ScansVariableCorrectlyWhenAtBeginningOfWord()
    {
        var token = TokenScanner.ScanToken("متغير ع = 1؛", 1, 1, 1, CharacterType.LITERAL);
        Assert.Equal(1, token.StartIndex);
        Assert.Equal(5, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(1, token.StartLineIndex);
        Assert.Equal(5, token.EndLineIndex);
        Assert.Equal("متغير", token.Label);
        Assert.Equal(TokenType.VAR, token.Type);
    }
    
    [Fact]
    private void ScansVariableCorrectlyWhenAtEndOfWord()
    {
        var token = TokenScanner.ScanToken("أكتب متغير", 6, 6, 1, CharacterType.LITERAL);
        Assert.Equal(6, token.StartIndex);
        Assert.Equal(10, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(6, token.StartLineIndex);
        Assert.Equal(10, token.EndLineIndex);
        Assert.Equal("متغير", token.Label);
        Assert.Equal(TokenType.VAR, token.Type);
    }
    
    [Fact]
    private void ScansConstCorrectly()
    {
        var token = TokenScanner.ScanToken("ثابت", 1, 1, 1, CharacterType.LITERAL);
        Assert.Equal(1, token.StartIndex);
        Assert.Equal(4, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal(1, token.StartLineIndex);
        Assert.Equal(4, token.EndLineIndex);
        Assert.Equal("ثابت", token.Label);
        Assert.Equal(TokenType.CONST, token.Type);
    }

    [Fact]
    private void ScansNumberCorrectly()
    {
        var token = TokenScanner.ScanToken("123", 1, 1, 1, CharacterType.LITERAL);
        Assert.Equal(1, token.StartIndex);
        Assert.Equal(3, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal("123", token.Label);
        Assert.Equal(1, token.StartLineIndex);
        Assert.Equal(3, token.EndLineIndex);
        Assert.Equal(TokenType.NUMBER_CONST, token.Type);
    }

    [Fact]
    private void ScansNumberCorrectlyWhenSurroundedWithCode()
    {
        var token = TokenScanner.ScanToken("متغير ا = 123؛", 11, 11, 1, CharacterType.LITERAL);
        Assert.Equal(11, token.StartIndex);
        Assert.Equal(13, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal("123", token.Label);
        Assert.Equal(11, token.StartLineIndex);
        Assert.Equal(13, token.EndLineIndex);
        Assert.Equal(TokenType.NUMBER_CONST, token.Type);
    }

    [Fact]
    private void ScansDecimalNumberCorrectlyWhenSurroundedWithCode()
    {
        var token = TokenScanner.ScanToken("متغير ا = 123.23؛", 11, 11, 1, CharacterType.LITERAL);
        Assert.Equal(11, token.StartIndex);
        Assert.Equal(16, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal("123.23", token.Label);
        Assert.Equal(11, token.StartLineIndex);
        Assert.Equal(16, token.EndLineIndex);
        Assert.Equal(TokenType.NUMBER_CONST, token.Type);
    }

    [Fact]
    private void ScansIdentifierCorrectly()
    {
        var token = TokenScanner.ScanToken("متغير ا؛", 7, 7, 1, CharacterType.LITERAL);
        Assert.Equal(7, token.StartIndex);
        Assert.Equal(7, token.EndIndex);
        Assert.Equal(1, token.StartLine);
        Assert.Equal("ا", token.Label);
        Assert.Equal(7, token.StartLineIndex);
        Assert.Equal(7, token.EndLineIndex);
        Assert.Equal(TokenType.ID, token.Type);   
    }

    [Fact]
    private void ThrowsExceptionWhenIdentifierBeginsWithDigit()
    {
        var exception = Assert.Throws<InvalidWordException>(() => TokenScanner.ScanToken("متغير 1ا؛", 7, 7, 1, CharacterType.LITERAL));
        Assert.Equal(7, exception.Index);
        Assert.Equal(1, exception.Line);
        Assert.Equal("1ا", exception.Label);
        Assert.Equal($"Invalid word: '1ا' at line 1 : 7", exception.EnglishMessage);   
        Assert.Equal($"كلمة غير صالحة: '1ا' على السطر 1 : 7", exception.ArabicMessage);   
    }
    
    [Fact]
    private void ScansVariableCorrectlyWhenProceededByCommend()
    {
        var token = TokenScanner.ScanToken("# هذا تعليق\nمتغير ج = عدم؛", 13, 1, 2, CharacterType.LITERAL);
        Assert.Equal(13, token.StartIndex);
        Assert.Equal(17, token.EndIndex);
        Assert.Equal(2, token.StartLine);
        Assert.Equal(1, token.StartLineIndex);
        Assert.Equal(5, token.EndLineIndex);
        Assert.Equal(TokenType.VAR, token.Type);
    }
}