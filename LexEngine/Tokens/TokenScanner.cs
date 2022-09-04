using System.Text;
using LexEngine.Characters;

namespace LexEngine.Tokens;

public static class TokenScanner
{
    public static Token? ScanToken(string code, int current, int line, CharacterType characterType)
    {
        return characterType switch
        {
            CharacterType.RIGHT_PAREN        => ScanSingletonToken(current, line, TokenType.OPEN_PAREN),
            CharacterType.LEFT_PAREN         => ScanSingletonToken(current, line, TokenType.CLOSE_PAREN),
            CharacterType.RIGHT_BRACE        => ScanSingletonToken(current, line, TokenType.OPEN_BRACE),
            CharacterType.LEFT_BRACE         => ScanSingletonToken(current, line, TokenType.CLOSE_BRACE),
            CharacterType.SEMICOLON          => ScanSingletonToken(current, line, TokenType.SEMICOLON),
            CharacterType.COMMA              => ScanSingletonToken(current, line, TokenType.COMMA),
            CharacterType.DOT                => ScanSingletonToken(current, line, TokenType.DOT),
            CharacterType.PERCENTAGE         => ScanSingletonToken(current, line, TokenType.MODULO),
            CharacterType.DASH               => ScanSingletonToken(current, line, TokenType.MINUS),
            CharacterType.PLUS               => ScanSingletonToken(current, line, TokenType.PLUS),
            CharacterType.STAR               => ScanSingletonToken(current, line, TokenType.TIMES),
            CharacterType.SLASH              => ScanSingletonToken(current, line, TokenType.DIVIDED_BY),
            CharacterType.EQUAL              => ScanEqualSign(code, current, line),
            CharacterType.EXCLAMATION_MARK   => ScanExclamationMark(code, current, line),
            CharacterType.RIGHT_SINGLE_ANGLE => ScanRightAngleSign(code, current, line),
            CharacterType.LEFT_SINGLE_ANGLE  => ScanLeftAngleSign(code, current, line),
            CharacterType.AMPERSAND          => ScanAmpersandSign(code, current, line),
            CharacterType.VERTICAL_BAR       => ScanVerticalBar(code, current, line),
            CharacterType.HASH               => ScanCommentToken(code, current, line),
            CharacterType.WHITE_SPACE        => ScanWhiteSpaceToken(code, current, line),
            CharacterType.DOUBLE_QUOTE       => ScanStringToken(code, current, line),
            CharacterType.LITERAL            => ScanLiteral(code, current, line),
            _                                => throw new InvalidTokenException(line, current)
        };
    }

    private static Token ScanSingletonToken(int current, int line, TokenType tokenType)
    {
        return new Token
        {
            StartIndex = current, EndIndex = current, Line = line, Type = tokenType
        };
    }

    private static Token? ScanLiteral(string code, int current, int line)
    {
        Token? token;
        if (TryScanWord(code, current, line, out token)) return token;
        if (TryScanNumber(code, current, line, out token)) return token;
        return ScanIdentifier(code, current, line);
    }

    private static Token? ScanIdentifier(string code, int current, int line)
    {
        var identifier = WordScanner.ScanNextWord(code, current - 1, line);
        return new Token
        {
            StartIndex = current,
            EndIndex = current + identifier.Length - 1,
            Line = line,
            Label = identifier,
            Type = TokenType.ID
        };
    }

    private static bool TryScanNumber(string code, int current, int line, out Token? token)
    {
        try
        {
            var number = WordScanner.ScanNextNumber(code, current - 1, line);
            token = new Token
            {
                StartIndex = current,
                EndIndex = current + number.Length - 1,
                Line = line,
                Label = number,
                Type = TokenType.NUMBER_CONST
            };
            return true;
        }
        catch (InvalidWordException)
        {
        }
        
        token = default;
        return false;
    }

    private static bool TryScanWord(string code, int current, int line, out Token? token)
    {
        try
        {
            var word = WordScanner.ScanNextWord(code, current - 1, line);
            if (KeywordsFactory.IsKeyword(word))
            {
                token = new Token
                {
                    StartIndex = current,
                    EndIndex = current + word.Length - 1,
                    Line = line,
                    Type = KeywordsFactory.GetToken(word)
                };
                return true;
            }
        }
        catch (InvalidWordException)
        {
        }
        
        token = default;
        return false;
    }

    private static Token? ScanStringToken(string code, int current, int line)
    {
        var stringIndex = current;
        var stringContent = new StringBuilder();
        while (code.Length > current && code[current] != '\"')
        {
            if (code[current] == '\n')
            {
                throw new MissingTokenException(current + 1, line, "\"");
            }

            stringContent.Append(code[current]);
            current++;
        }

        if (code.Length > current && code[current] == '\"') current++;
        else
        {
            throw new MissingTokenException(current, line, "\"");
        }

        return new Token
        {
            StartIndex = stringIndex, EndIndex = current, Label = stringContent.ToString(), Line = line,
            Type = TokenType.STRING_CONST
        };
    }

    private static Token? ScanWhiteSpaceToken(string code, int current, int line)
    {
        var spaceIndex = current;
        while (code.Length > current &&
               CharacterAnalyzer.AnalyzeCharacterType(code[current]) == CharacterType.WHITE_SPACE)
        {
            current++;
        }

        return new Token {StartIndex = spaceIndex, EndIndex = current, Line = line, Type = TokenType.WHITE_SPACE};
    }

    private static Token? ScanCommentToken(string code, int current, int line)
    {
        var comment = new StringBuilder();
        var commentIndex = current;
        while (code.Length > current && code[current] != '\n')
        {
            comment.Append(code[current]);
            current++;
        }

        return new Token
        {
            StartIndex = commentIndex, EndIndex = current, Line = line, Label = comment.ToString(), Type = TokenType.COMMENT
        };
    }

    private static Token? ScanVerticalBar(string code, int current, int line)
    {
        if (code.Length > current && code[current] == '|')
        {
            return new Token {StartIndex = current, EndIndex = current + 1, Line = line, Type = TokenType.OR};
        }
        else
        {
            throw new MissingTokenException(current, line, "|");
        }
    }

    private static Token? ScanAmpersandSign(string code, int current, int line)
    {
        if (code.Length > current && code[current] == '&')
        {
            return new Token
                {StartIndex = current, EndIndex = current + 1, Line = line, Type = TokenType.AND};
        }
        else
        {
            throw new MissingTokenException(current, line, "&");
        }
    }

    private static Token? ScanLeftAngleSign(string code, int current, int line)
    {
        if (code.Length > current && code[current] == '=')
        {
            return new Token
                {StartIndex = current, EndIndex = current + 1, Line = line, Type = TokenType.GREATER_EQUAL};
        }
        else
        {
            return new Token
                {StartIndex = current, EndIndex = current, Line = line, Type = TokenType.GREATER_THAN};
        }
    }

    private static Token? ScanRightAngleSign(string code, int current, int line)
    {
        if (code.Length > current && code[current] == '=')
        {
            return new Token
                {StartIndex = current, EndIndex = current + 1, Line = line, Type = TokenType.LESS_EQUAL};
        }
        else
        {
            return new Token
                {StartIndex = current, EndIndex = current, Line = line, Type = TokenType.LESS_THAN};
        }
    }

    private static Token? ScanExclamationMark(string code, int current, int line)
    {
        if (code.Length > current && code[current] == '=')
        {
            return new Token
                {StartIndex = current, EndIndex = current + 1, Line = line, Type = TokenType.BANG_EQUAL};
        }
        else
        {
            return new Token {StartIndex = current, EndIndex = current, Line = line, Type = TokenType.BANG};
        }
    }

    private static Token? ScanEqualSign(string code, int current, int line)
    {
        if (code.Length > current && code[current] == '=')
        {
            return new Token
                {StartIndex = current, EndIndex = current + 1, Line = line, Type = TokenType.EQUAL_EQUAL};
        }
        else
        {
            return new Token {StartIndex = current, EndIndex = current, Line = line, Type = TokenType.EQUAL};
        }
    }
}