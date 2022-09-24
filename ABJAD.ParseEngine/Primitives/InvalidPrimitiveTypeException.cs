using ABJAD.ParseEngine.Shared;

namespace ABJAD.ParseEngine.Primitives;

public class InvalidPrimitiveTypeException : Exception
{
    public TokenType TokenType { get; }
    public int Line { get; }
    public int Index { get; }

    public InvalidPrimitiveTypeException(TokenType tokenType, int line, int index)
    {
        TokenType = tokenType;
        Line = line;
        Index = index;
    }
}