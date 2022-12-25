namespace ABJAD.InterpretEngine.Expressions;

public class NotInstanceReferenceException : InterpretationException
{
    public NotInstanceReferenceException(string reference) 
        : base(FormulateArabicMessage(reference), FormulateEnglishMessage(reference))
    {
    }

    private static string FormulateArabicMessage(string reference)
    {
        return $"المؤشر {reference} ليس وحدة من صنف معين.";
    }

    private static string FormulateEnglishMessage(string reference)
    {
        return $"Reference {reference} is not an instance of a class.";
    }
}