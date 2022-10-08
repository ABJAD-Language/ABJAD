using ABJAD.LexEngine.Tokens;

namespace ABJAD.LexEngine.Scanning;

public class ScanStarStrategy : ScanningStrategy
{
    public Token Scan(string code, int current, int line, int lineIndex)
    {
        if (code.Length > current && code[current] == '=')
        {
            return ScanStarEqualToken(current, line, lineIndex);
        }

        return ScanStarToken(current, line, lineIndex);
    }

    private static Token ScanStarToken(int current, int line, int lineIndex)
    {
        return new Token
        {
            StartIndex = current, EndIndex = current, StartLine = line, StartLineIndex = lineIndex,
            EndLineIndex = lineIndex, Type = TokenType.STAR, Label = "*"
        };
    }

    private static Token ScanStarEqualToken(int current, int line, int lineIndex)
    {
        return new Token
        {
            StartIndex = current, EndIndex = current + 1, StartLine = line, StartLineIndex = lineIndex,
            EndLineIndex = lineIndex + 1, Type = TokenType.STAR_EQUAL, Label = "*="
        };
    }
}