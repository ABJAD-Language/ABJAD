namespace ABJAD.ParseEngine;

public abstract class ParsingException : Exception
{
    protected ParsingException(string message) : base(message)
    {
    }

    public string ArabicMessage { get; set;  }
    public string EnglishMessage { get; set; }

    public int Line { get; set; }
    public int Index { get; set; }
}