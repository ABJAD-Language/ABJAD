namespace ABJAD.LexEngine;

public abstract class LexicalAnalysisException : Exception
{
    protected LexicalAnalysisException(string message) : base(message)
    {
    }
    
    public abstract string ArabicMessage { get; }
    public abstract string EnglishMessage { get; }
}