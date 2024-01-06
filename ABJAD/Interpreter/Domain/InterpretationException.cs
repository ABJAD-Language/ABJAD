namespace ABJAD.Interpreter.Domain;

public class InterpretationException : Exception
{
    public string ArabicMessage { get; }
    public string EnglishMessage { get; }

    public InterpretationException(string arabicMessage, string englishMessage) : base(englishMessage)
    {
        ArabicMessage = arabicMessage;
        EnglishMessage = englishMessage;
    }
}