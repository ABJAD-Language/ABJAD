namespace ABJAD.InterpretEngine.Declarations;

public class ConstantDeclarationFailureException : InterpretationException
{
    public ConstantDeclarationFailureException() 
        : base("القيمة المسموح تعيينها لمؤشر ثابت يجب أن تكون بدائية (رقم، مقطع، أو منطق).", 
            "The value of a constant declaration should be a primitive (string, number, or bool).")
    {
    }
}