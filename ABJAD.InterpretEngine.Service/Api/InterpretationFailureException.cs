namespace ABJAD.InterpretEngine.Service.Api;

public class InterpretationFailureException : InterpretationException
{
    public InterpretationFailureException()
        : base(FormulateArabicMessage(), FormulateEnglishMessage())
    {
    }

    private static string FormulateArabicMessage()
    {
        return "لقد واجهنا مشكلة خلال محاولة تشغيل الكود. قد يكون هناك خطأ في الكود نفسه، أو عطلٌ من جانبنا.";
    }

    private static string FormulateEnglishMessage()
    {
        return "We have encountered an issue while interpreting your code. It might be a problem from your code, or from our side.";
    }
}