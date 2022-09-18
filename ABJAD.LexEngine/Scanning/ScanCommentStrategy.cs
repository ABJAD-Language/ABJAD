using System.Text;
using ABJAD.LexEngine.Tokens;

namespace ABJAD.LexEngine.Scanning;

public class ScanCommentStrategy : ScanningStrategy
{
    public Token Scan(string code, int current, int line, int lineIndex)
    {
        var comment = new StringBuilder();
        var commentIndex = current;
        var endLineIndex = lineIndex;
        while (code.Length > current && code[current] != '\n')
        {
            comment.Append(code[current]);
            current++;
            endLineIndex++;
        }

        return new Token
        {
            StartIndex = commentIndex, EndIndex = current, StartLine = line, StartLineIndex = lineIndex,
            EndLineIndex = endLineIndex, Label = comment.ToString(), Type = TokenType.COMMENT
        };
    }
}