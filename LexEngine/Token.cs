namespace LexEngine;

public class Token
{
    public TokenType Type { get; set; }
    public string Label { get; set; } = null!;
    public int? Line { get; set; }
    public int? Index { get; set; }
}