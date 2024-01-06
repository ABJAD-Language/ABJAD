namespace ABJAD.Interpreter.Domain;

public class NullPointerException : InterpretationException
{
    public NullPointerException() :
        base("قيمة غير موجودة: تحاولون الوصول إلى قيمة غير موجودة (عدم).",
            "Null reference exception: you're trying to reference a null value.")
    {
    }
}