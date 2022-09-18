using ABJAD.LexEngine.Tokens;

namespace ABJAD.LexEngine.Scanning;

public class ScanRightAngleSignStrategy : ScanningStrategy
{
    public Token Scan(string code, int current, int line, int lineIndex)
    {
        if (code.Length > current && code[current] == '=')
        {
            return ScanLessEqualToken(current, line, lineIndex);
        }

        return ScanLessToken(current, line, lineIndex);
    }

    private static Token ScanLessToken(int current, int line, int lineIndex)
    {
        return new Token
        {
            StartIndex = current, EndIndex = current, StartLine = line, StartLineIndex = lineIndex,
            EndLineIndex = lineIndex, Label = "<", Type = TokenType.LESS_THAN
        };
    }

    private static Token ScanLessEqualToken(int current, int line, int lineIndex)
    {
        return new Token
        {
            StartIndex = current, EndIndex = current + 1, StartLine = line, StartLineIndex = lineIndex,
            EndLineIndex = lineIndex + 1, Label = "<=", Type = TokenType.LESS_EQUAL
        };
    }
}