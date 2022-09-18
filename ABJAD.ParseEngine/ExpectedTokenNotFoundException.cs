using ABJAD.ParseEngine.Shared;

namespace ABJAD.ParseEngine;

public class ExpectedTokenNotFoundException : ParsingException
{
    public ExpectedTokenNotFoundException(int line, int index, TokenType targetType) : base(FormulateEnglishMessage(line, index, targetType))
    {
        Line = line;
        Index = index;
        ArabicMessage = FormulateArabicMessage(line, index, targetType);
        EnglishMessage = FormulateEnglishMessage(line, index, targetType);
    }

    private static string FormulateEnglishMessage(int line, int index, TokenType type)
    {
        return $"Expected token of type '{type}' was not found at line {line}:{index}";
    }

    private static string FormulateArabicMessage(int line, int index, TokenType type)
    {
        return $"رمز متوقع من نوع '{type}' لم يوجد على السطر {line}:{index}";
    }
    
}