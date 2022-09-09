namespace LexEngine.Service.Core;

public class LexicalToken
{
    public int Line { get; set; }
    public int Index { get; set; }
    public string Type { get; set; }
    public string Content { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is LexicalToken otherToken && Line == otherToken.Line && Index == otherToken.Index && Type == otherToken.Type &&
               Content == otherToken.Content;
    }
}