using ABJAD.Interpreter.Domain.Types;

namespace ABJAD.Interpreter.Domain.ScopeManagement;

public class MatchingConstructorExistsException : InterpretationException
{
    public MatchingConstructorExistsException(string className, params DataType[] paramTypes)
        : base(FormulateArabicMessage(className, paramTypes), FormulateEnglishMessage(className, paramTypes))
    {
    }

    private static string FormulateArabicMessage(string className, DataType[] paramTypes)
    {
        return $"منشئ للصنف {className} والذي يستقبل المعطيات [{string.Join("، ", paramTypes.Select(p => p.GetValue()))}] موجود فعلا.";
    }

    private static string FormulateEnglishMessage(string className, DataType[] paramTypes)
    {
        return $"A constructor for the class {className} that takes the parameters [{string.Join(", ", paramTypes.Select(p => p.GetValue()))}] already exists.";
    }
}