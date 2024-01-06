using ABJAD.Interpreter.Domain.Types;

namespace ABJAD.Interpreter.Domain.ScopeManagement;

public class NoSuitableConstructorFoundException : InterpretationException
{
    public NoSuitableConstructorFoundException(string className, params DataType[] parameterTypes)
        : base(FormulateArabicMessage(className, parameterTypes), FormulateEnglishMessage(className, parameterTypes))
    {
    }

    private static string FormulateArabicMessage(string className, params DataType[] parameterTypes)
    {
        return $"لا يوجد منشئ للصنف {className} يأخذ المعطيات التالية [{string.Join("، ", parameterTypes.Select(p => p.GetValue()))}].";
    }

    private static string FormulateEnglishMessage(string className, params DataType[] parameterTypes)
    {
        return $"Did not find a constructor for class {className} that takes [{string.Join(", ", parameterTypes.Select(p => p.GetValue()))}] as parameters.";
    }
}

