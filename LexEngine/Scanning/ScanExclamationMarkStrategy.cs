using LexEngine.Tokens;

namespace LexEngine.Scanning;

public class ScanExclamationMarkStrategy : ScanningStrategy
{
    public Token Scan(string code, int current, int line, int lineIndex)
    {
        if (code.Length > current && code[current] == '=')
        {
            return ScanBangEqualToken(current, line, lineIndex);
        }

        return ScanBangToken(current, line, lineIndex);
    }

    private static Token ScanBangToken(int current, int line, int lineIndex)
    {
        return new Token
        {
            StartIndex = current, EndIndex = current, StartLine = line, StartLineIndex = lineIndex,
            EndLineIndex = lineIndex, Label = "!", Type = TokenType.BANG
        };
    }

    private static Token ScanBangEqualToken(int current, int line, int lineIndex)
    {
        return new Token
        {
            StartIndex = current, EndIndex = current + 1, StartLine = line, StartLineIndex = lineIndex,
            EndLineIndex = lineIndex + 1, Label = "!=", Type = TokenType.BANG_EQUAL
        };
    }
}