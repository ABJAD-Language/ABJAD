using ABJAD.LexEngine.Tokens;

namespace ABJAD.LexEngine.Scanning;

public class ScanColonStrategy : ScanningStrategy
{
    public Token Scan(string code, int current, int line, int lineIndex)
    {
        return new Token
        {
            Type = TokenType.COLON, StartIndex = current, EndIndex = current, StartLine = line, 
            StartLineIndex = lineIndex, EndLineIndex = lineIndex, Label = ":"
        };
    }
}