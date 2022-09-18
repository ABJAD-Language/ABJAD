using System.Text;
using LexEngine.Tokens;

namespace LexEngine.Scanning;

public class ScanCommentStrategy : ScanningStrategy
{
    public Token Scan(string code, int current, int line, int lineIndex)
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
            StartIndex = commentIndex, EndIndex = current, StartLine = line, StartLineIndex = lineIndex,
            Label = comment.ToString(), Type = TokenType.COMMENT
        };
    }
}