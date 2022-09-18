namespace ABJAD.LexEngine;

public class StringUtils
{
    public virtual string IgnoreCaseSensitivity(string text)
    {
        return text
            .Replace("ـ", "")
            .Replace("أ", "ا")
            .Replace("إ", "ا")
            .Replace("آ", "ا");
    }
}