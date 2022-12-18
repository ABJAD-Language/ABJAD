namespace ABJAD.InterpretEngine;

public class InvalidReferenceException : InterpretationException
{
    private InvalidReferenceException(string arabicMessage, string englishMessage) : base(arabicMessage, englishMessage)
    {
    }

    public static InvalidReferenceException IdentifierRequired()
    {
        return new InvalidReferenceException("لا يمكن إجراء هذه العملية إلا على مؤشر.", "This operation can only be done on identifiers.");
    }
}