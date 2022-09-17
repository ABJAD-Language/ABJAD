using System.Text;
using LexEngine.Characters;

namespace LexEngine.Tokens;

public static class TokenScanner
{
    public static Token ScanToken(string code, int current, int startLineIndex, int line, CharacterType characterType)
    {
        return characterType switch
        {
            CharacterType.RIGHT_PAREN        => ScanSingletonToken(current, line, startLineIndex, "(", TokenType.OPEN_PAREN),
            CharacterType.LEFT_PAREN         => ScanSingletonToken(current, line, startLineIndex, ")", TokenType.CLOSE_PAREN),
            CharacterType.RIGHT_BRACE        => ScanSingletonToken(current, line, startLineIndex, "{", TokenType.OPEN_BRACE),
            CharacterType.LEFT_BRACE         => ScanSingletonToken(current, line, startLineIndex, "}", TokenType.CLOSE_BRACE),
            CharacterType.SEMICOLON          => ScanSingletonToken(current, line, startLineIndex, "؛", TokenType.SEMICOLON),
            CharacterType.COMMA              => ScanSingletonToken(current, line, startLineIndex, "،", TokenType.COMMA),
            CharacterType.DOT                => ScanSingletonToken(current, line, startLineIndex, ".", TokenType.DOT),
            CharacterType.PERCENTAGE         => ScanSingletonToken(current, line, startLineIndex, "%", TokenType.MODULO),
            CharacterType.DASH               => ScanSingletonToken(current, line, startLineIndex, "-", TokenType.MINUS),
            CharacterType.PLUS               => ScanSingletonToken(current, line, startLineIndex, "+", TokenType.PLUS),
            CharacterType.STAR               => ScanSingletonToken(current, line, startLineIndex, "*", TokenType.TIMES),
            CharacterType.SLASH              => ScanSingletonToken(current, line, startLineIndex, "\\", TokenType.DIVIDED_BY),
            CharacterType.EQUAL              => ScanEqualSign(code, current, line, startLineIndex),
            CharacterType.EXCLAMATION_MARK   => ScanExclamationMark(code, current, line, startLineIndex),
            CharacterType.RIGHT_SINGLE_ANGLE => ScanRightAngleSign(code, current, line, startLineIndex),
            CharacterType.LEFT_SINGLE_ANGLE  => ScanLeftAngleSign(code, current, line, startLineIndex),
            CharacterType.AMPERSAND          => ScanAmpersandSign(code, current, line, startLineIndex),
            CharacterType.VERTICAL_BAR       => ScanVerticalBar(code, current, line, startLineIndex),
            CharacterType.HASH               => ScanCommentToken(code, current, line, startLineIndex),
            CharacterType.WHITE_SPACE        => ScanWhiteSpaceToken(code, current, line, startLineIndex),
            CharacterType.DOUBLE_QUOTE       => ScanStringToken(code, current, line, startLineIndex),
            CharacterType.LITERAL            => ScanLiteral(code, current, line, startLineIndex),
            _                                => throw new InvalidTokenException(line, startLineIndex)
        };
    }

    private static Token ScanSingletonToken(int current, int line, int startLineIndex, string label, TokenType tokenType)
    {
        return new Token
        {
            StartIndex = current, EndIndex = current, StartLine = line, StartLineIndex = startLineIndex,
            EndLineIndex = startLineIndex, Type = tokenType, Label = label
        };
    }

    private static Token ScanLiteral(string code, int current, int line, int lineIndex)
    {
        Token? token;
        if (TryScanWord(code, current, line, lineIndex, out token)) return token;
        if (TryScanNumber(code, current, line, lineIndex, out token)) return token;
        return ScanIdentifier(code, current, line, lineIndex);
    }

    private static Token ScanIdentifier(string code, int current, int line, int lineIndex)
    {
        var identifier = WordScanner.ScanNextWord(code, current - 1, line);
        return new Token
        {
            StartIndex = current,
            EndIndex = current + identifier.Length - 1,
            StartLine = line,
            StartLineIndex = lineIndex,
            EndLineIndex = lineIndex + identifier.Length - 1,
            Label = identifier,
            Type = TokenType.ID
        };
    }

    private static bool TryScanNumber(string code, int current, int line, int lineIndex, out Token? token)
    {
        try
        {
            var number = WordScanner.ScanNextNumber(code, current - 1, line);
            token = new Token
            {
                StartIndex = current,
                EndIndex = current + number.Length - 1,
                StartLine = line,
                StartLineIndex = lineIndex,
                EndLineIndex = lineIndex + number.Length - 1,
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

    private static bool TryScanWord(string code, int current, int line, int lineIndex, out Token? token)
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
                    StartLine = line,
                    StartLineIndex = lineIndex,
                    EndLineIndex = lineIndex + word.Length - 1,
                    Label = word,
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

    private static Token ScanStringToken(string code, int current, int line, int lineIndex)
    {
        var stringIndex = current;
        var stringContent = new StringBuilder();
        var beginningLineIndex = lineIndex;
        while (code.Length > current && code[current] != '\"')
        {
            if (code[current] == '\n')
            {
                throw new MissingTokenException(lineIndex + 1, line, "\"");
            }

            if (code[current] == '\\')
            {
                if (code.Length > current + 1)
                {
                    if (code[current + 1] == '"' || code[current + 1] == '\\')
                    {
                        current++;
                        lineIndex++;
                    }
                    else
                    {
                        throw new InvalidTokenException(line, lineIndex + 1);
                    }
                }
            }

            stringContent.Append(code[current]);
            current++;
            lineIndex++;
        }

        if (code.Length > current && code[current] == '\"')
        {
            current++;
            lineIndex++;
        }
        else
        {
            throw new MissingTokenException(lineIndex, line, "\"");
        }

        return new Token
        {
            StartIndex = stringIndex, EndIndex = current, Label = stringContent.ToString(), StartLine = line,
            StartLineIndex = beginningLineIndex, EndLineIndex = lineIndex, Type = TokenType.STRING_CONST
        };
    }

    private static Token ScanWhiteSpaceToken(string code, int current, int line, int lineIndex)
    {
        var spaceIndex = current;
        var spaceLine = line;
        var beginningLineIndex = lineIndex;
        var content = new StringBuilder();
        if (code[current-1] == '\n')
        {
            line++;
            lineIndex = 1;
        }
        content.Append(code[current-1]);
        while (code.Length > current &&
               CharacterAnalyzer.AnalyzeCharacterType(code[current]) == CharacterType.WHITE_SPACE)
        {
            if (code[current] == '\n')
            {
                line++;
                lineIndex = 1;
            }
            else
            {
                lineIndex++;
            }
            content.Append(code[current]);
            current++;
        }

        return new Token
        {
            StartIndex = spaceIndex, EndIndex = current, StartLine = spaceLine, StartLineIndex = beginningLineIndex,
            EndLineIndex = lineIndex, EndLine = line, Label = content.ToString(), Type = TokenType.WHITE_SPACE
        };
    }

    private static Token ScanCommentToken(string code, int current, int line, int lineIndex)
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
            StartIndex = commentIndex, EndIndex = current, StartLine = line, StartLineIndex = lineIndex, Label = comment.ToString(), Type = TokenType.COMMENT
        };
    }

    private static Token ScanVerticalBar(string code, int current, int line, int lineIndex)
    {
        if (code.Length > current && code[current] == '|')
        {
            return new Token
            {
                StartIndex = current, EndIndex = current + 1, StartLine = line, StartLineIndex = lineIndex,
                EndLineIndex = lineIndex + 1, Label = "||", Type = TokenType.OR
            };
        }

        throw new MissingTokenException(lineIndex, line, "|");
    }

    private static Token ScanAmpersandSign(string code, int current, int line, int lineIndex)
    {
        if (code.Length > current && code[current] == '&')
        {
            return new Token
            {
                StartIndex = current, EndIndex = current + 1, StartLine = line, StartLineIndex = lineIndex,
                EndLineIndex = lineIndex + 1, Label = "&&", Type = TokenType.AND
            };
        }

        throw new MissingTokenException(lineIndex, line, "&");
    }

    private static Token ScanLeftAngleSign(string code, int current, int line, int lineIndex)
    {
        if (code.Length > current && code[current] == '=')
        {
            return new Token
            {
                StartIndex = current, EndIndex = current + 1, StartLine = line, StartLineIndex = lineIndex,
                EndLineIndex = lineIndex + 1, Label = ">=", Type = TokenType.GREATER_EQUAL
            };
        }

        return new Token
        {
            StartIndex = current, EndIndex = current, StartLine = line, StartLineIndex = lineIndex,
            EndLineIndex = lineIndex, Label = ">", Type = TokenType.GREATER_THAN
        };
    }

    private static Token ScanRightAngleSign(string code, int current, int line, int lineIndex)
    {
        if (code.Length > current && code[current] == '=')
        {
            return new Token
            {
                StartIndex = current, EndIndex = current + 1, StartLine = line, StartLineIndex = lineIndex,
                EndLineIndex = lineIndex + 1, Label = "<=", Type = TokenType.LESS_EQUAL
            };
        }

        return new Token
        {
            StartIndex = current, EndIndex = current, StartLine = line, StartLineIndex = lineIndex,
            EndLineIndex = lineIndex, Label = "<", Type = TokenType.LESS_THAN
        };
    }

    private static Token ScanExclamationMark(string code, int current, int line, int lineIndex)
    {
        if (code.Length > current && code[current] == '=')
        {
            return new Token
            {
                StartIndex = current, EndIndex = current + 1, StartLine = line, StartLineIndex = lineIndex,
                EndLineIndex = lineIndex + 1, Label = "!=", Type = TokenType.BANG_EQUAL
            };
        }

        return new Token
        {
            StartIndex = current, EndIndex = current, StartLine = line, StartLineIndex = lineIndex,
            EndLineIndex = lineIndex, Label = "!", Type = TokenType.BANG
        };
    }

    private static Token ScanEqualSign(string code, int current, int line, int lineIndex)
    {
        if (code.Length > current && code[current] == '=')
        {
            return new Token
            {
                StartIndex = current, EndIndex = current + 1, StartLine = line, StartLineIndex = lineIndex,
                EndLineIndex = lineIndex + 1, Label = "==", Type = TokenType.EQUAL_EQUAL
            };
        }

        return new Token
        {
            StartIndex = current, EndIndex = current, StartLine = line, StartLineIndex = lineIndex,
            EndLineIndex = lineIndex, Label = "=", Type = TokenType.EQUAL
        };
    }
}