namespace LexEngine.Tokens;

public class InvalidWordException : LexicalAnalysisException
{
    public InvalidWordException(int index, int line, string label) : base(FormulateEnglishMessage(index, line, label))
    {
        Index = index;
        Line = line;
        Label = label;
        EnglishMessage = FormulateEnglishMessage(index, line, label);
        ArabicMessage = FormulateArabicMessage(index, line, label);
    }

    private static string FormulateArabicMessage(int index, int line, string label)
    {
        return $"كلمة غير صالحة: '{label}' على السطر {line} : {index}";
    }

    private static string FormulateEnglishMessage(int index, int line, string label)
    {
        return $"Invalid word: '{label}' at line {line} : {index}";
    }

    public int Index { get; }

    public int Line { get; }

    public string Label { get; }
    public override string EnglishMessage { get; }
    public override string ArabicMessage { get; }
}