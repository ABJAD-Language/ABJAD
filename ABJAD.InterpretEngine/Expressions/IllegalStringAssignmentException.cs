namespace ABJAD.InterpretEngine.Expressions;

public class IllegalStringAssignmentException : InterpretationException
{
    public IllegalStringAssignmentException() 
        : base(FormulateArabicMessage(), FormulateEnglishMessage())
    {
    }

    private static string FormulateArabicMessage()
    {
        return "العمليات الحسابية المُعدّلة للقيمة يجوز استعمالها على المقاطع فقط في الحالتين" +
               " التاليتين: عملية الجمع بحيث تكون القيمة المضافة مقطعًا، أو في عملية الضرب حيث تكون القيمة المضروبة رقمًا.";
    }

    private static string FormulateEnglishMessage()
    {
        return "Assignment expressions can be applied to strings only in the case of addition assignment expression " +
               "where the added value is also a string, or multiplication assignment expression where the multiplied value is a number.";
    }
}