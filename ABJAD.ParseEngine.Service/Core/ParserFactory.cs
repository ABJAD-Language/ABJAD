using ABJAD.ParseEngine.Shared;

namespace ABJAD.ParseEngine.Service.Core;

public interface ParserFactory
{
    IParser Get(List<Token> tokens);
}