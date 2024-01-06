using ABJAD.Interpreter.Domain.Types;

namespace ABJAD.Interpreter.Domain.Expressions;

public class NoValueReturnedException : InterpretationException
{
    public NoValueReturnedException(string methodName, DataType expectedReturnType)
        : base(FormulateArabicMessage(methodName, expectedReturnType), FormulateEnglishMessage(methodName, expectedReturnType))
    {
    }

    private static string FormulateArabicMessage(string methodName, DataType expectedReturnType)
    {
        return $"الدالة {methodName} كانت من المتوقع أن ترجع قيمة من نوع {expectedReturnType.GetValue()}، لكنها لم تفعل.";
    }

    private static string FormulateEnglishMessage(string methodName, DataType expectedReturnType)
    {
        return $"Method {methodName} was expected to return a value of type {expectedReturnType.GetValue()}, but did not.";
    }
}