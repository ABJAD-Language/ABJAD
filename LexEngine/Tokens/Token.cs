namespace LexEngine.Tokens;

public class Token
{
    public TokenType Type { get; set; }
    public string Label { get; set; } = null!;
    public int? Line { get; set; }
    public int? StartIndex { get; set; }
    public int? EndIndex { get; set; }
}