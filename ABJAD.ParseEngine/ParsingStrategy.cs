using ABJAD.ParseEngine.Shared;

namespace ABJAD.ParseEngine;

public interface ParsingStrategy<T>
{
    T Parse(List<Token> tokens, int index);
}