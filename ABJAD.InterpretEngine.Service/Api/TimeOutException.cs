namespace ABJAD.InterpretEngine.Service.Api;

public class TimeOutException : InterpretationException
{
    public TimeOutException() 
        : base(FormulateArabicMessage(), FormulateEnglishMessage())
    {
    }

    private static string FormulateEnglishMessage()
    {
        return "Your request has timed out. Check your code for any bugs causing it not to exit, then try again.";
    }

    private static string FormulateArabicMessage()
    {
        return "لقد تخطّى الكود المهلة المسموح بها للتشغيل. تحقق من أي أخطاء تمنعه من الانتهاء وحاول مرّة أخرى.";
    }
}