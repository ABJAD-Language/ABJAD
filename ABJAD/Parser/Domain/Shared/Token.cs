namespace ABJAD.Parser.Domain.Shared;

public class Token
{
    public int Line { get; set; }
    public int Index { get; set; }
    public TokenType Type { get; set; }
    public string Content { get; set; }
}