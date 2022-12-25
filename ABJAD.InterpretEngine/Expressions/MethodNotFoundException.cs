
using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.Expressions;

public class MethodNotFoundException : InterpretationException
{
    public MethodNotFoundException(string name, params DataType[] paramTypes) 
        : base(FormulateArabicMessage(name, paramTypes), FormulateEnglishMessage(name, paramTypes))
    {
    }

    private static string FormulateArabicMessage(string name, params DataType[] paramTypes)
    {
        return $"لا يوجد في المجال دالة باسم {name} تأخذ المعطيات ({string.Join("، ", paramTypes.Select(p => p.GetValue()))}).";
    }

    private static string FormulateEnglishMessage(string name, params DataType[] paramTypes)
    {
        return $"Method with name {name} that takes the parameters ({string.Join(", ", paramTypes.Select(p => p.GetValue()))}) does not exist.";
    }
}