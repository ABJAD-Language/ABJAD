namespace ABJAD.LexEngine.Tokens;

public class Token
{
    public TokenType Type { get; set; }
    public string Label { get; set; } = null!;
    public int? StartLine { get; set; }
    public int? EndLine { get; set; }
    public int? StartIndex { get; set; }
    public int? EndIndex { get; set; }
    public int StartLineIndex { get; set; }
    public int EndLineIndex { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Token otherToken && Type == otherToken.Type && Label == otherToken.Label &&
               StartLine == otherToken.StartLine && EndLine == otherToken.EndLine &&
               StartIndex == otherToken.StartIndex &&
               EndIndex == otherToken.EndIndex && StartLineIndex == otherToken.StartLineIndex &&
               EndLineIndex == otherToken.EndLineIndex;
    }
}