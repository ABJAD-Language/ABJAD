using ABJAD.LexEngine.Tokens;

namespace ABJAD.LexEngine.Scanning;

public class ScanSlashStrategy : ScanningStrategy
{
    public Token Scan(string code, int current, int line, int lineIndex)
    {
        if (code.Length > current && code[current] == '=')
        {
            return ScanSlashEqualToken(current, line, lineIndex);
        }

        return ScanSlashToken(current, line, lineIndex);
    }

    private static Token ScanSlashToken(int current, int line, int lineIndex)
    {
        return new Token
        {
            StartIndex = current, EndIndex = current, StartLine = line, StartLineIndex = lineIndex,
            EndLineIndex = lineIndex, Type = TokenType.SLASH, Label = "\\"
        };
    }

    private static Token ScanSlashEqualToken(int current, int line, int lineIndex)
    {
        return new Token
        {
            StartIndex = current, EndIndex = current + 1, StartLine = line, StartLineIndex = lineIndex,
            EndLineIndex = lineIndex + 1, Type = TokenType.SLASH_EQUAL, Label = "\\="
        };
    }
}