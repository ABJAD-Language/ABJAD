using LexEngine.Tokens;

namespace LexEngine.Scanning;

public class ScanLiteralStrategy : ScanningStrategy
{
    public Token Scan(string code, int current, int line, int lineIndex)
    {
        Token? token;
        if (TryScanWord(code, current, line, lineIndex, out token)) return token;
        if (TryScanNumber(code, current, line, lineIndex, out token)) return token;
        return ScanIdentifier(code, current, line, lineIndex);
    }

    private static bool TryScanNumber(string code, int current, int line, int lineIndex, out Token? token)
    {
        try
        {
            token = ScanNumber(code, current, line, lineIndex);
            return true;
        }
        catch (InvalidWordException)
        {
        }
        
        token = default;
        return false;
    }

    private static Token ScanNumber(string code, int current, int line, int lineIndex)
    {
        var number = WordScanner.ScanNextNumber(code, current - 1, line);
        return new Token
        {
            StartIndex = current,
            EndIndex = current + number.Length - 1,
            StartLine = line,
            StartLineIndex = lineIndex,
            EndLineIndex = lineIndex + number.Length - 1,
            Label = number,
            Type = TokenType.NUMBER_CONST
        };
    }

    private static bool TryScanWord(string code, int current, int line, int lineIndex, out Token? token)
    {
        try
        {
            token = ScanWord(code, current, line, lineIndex);
            return token != default;
        }
        catch (InvalidWordException)
        {
        }
        
        token = default;
        return false;
    }

    private static Token? ScanWord(string code, int current, int line, int lineIndex)
    {
        var word = WordScanner.ScanNextWord(code, current - 1, line);
        if (!KeywordsFactory.IsKeyword(word))
        {
            return default;
        }
        
        return new Token
        {
            StartIndex = current,
            EndIndex = current + word.Length - 1,
            StartLine = line,
            StartLineIndex = lineIndex,
            EndLineIndex = lineIndex + word.Length - 1,
            Label = word,
            Type = KeywordsFactory.GetToken(word)
        };
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
}