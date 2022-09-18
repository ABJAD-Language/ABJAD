using LexEngine.Tokens;

namespace LexEngine.Scanning;

public class ScanLeftAngleSignStrategy : ScanningStrategy
{
    public Token Scan(string code, int current, int line, int lineIndex)
    {
        if (code.Length > current && code[current] == '=')
        {
            return ScanGreaterEqualToken(current, line, lineIndex);
        }

        return ScanGreaterToken(current, line, lineIndex);
    }

    private static Token ScanGreaterToken(int current, int line, int lineIndex)
    {
        return new Token
        {
            StartIndex = current, EndIndex = current, StartLine = line, StartLineIndex = lineIndex,
            EndLineIndex = lineIndex, Label = ">", Type = TokenType.GREATER_THAN
        };
    }

    private static Token ScanGreaterEqualToken(int current, int line, int lineIndex)
    {
        return new Token
        {
            StartIndex = current, EndIndex = current + 1, StartLine = line, StartLineIndex = lineIndex,
            EndLineIndex = lineIndex + 1, Label = ">=", Type = TokenType.GREATER_EQUAL
        };
    }
}