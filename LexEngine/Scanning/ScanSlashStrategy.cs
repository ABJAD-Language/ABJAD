using LexEngine.Tokens;

namespace LexEngine.Scanning;

public class ScanSlashStrategy : ScanningStrategy
{
    public Token Scan(string code, int current, int line, int lineIndex)
    {
        return new Token
        {
            StartIndex = current, EndIndex = current, StartLine = line, StartLineIndex = lineIndex,
            EndLineIndex = lineIndex, Type = TokenType.SLASH, Label = "\\"
        };
    }
}