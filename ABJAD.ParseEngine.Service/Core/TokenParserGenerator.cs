using ABJAD.ParseEngine.Shared;

namespace ABJAD.ParseEngine.Service.Core;

public class TokenParserGenerator : ParserFactory
{
    public IParser Get(List<Token> tokens)
    {
        return new Parser(tokens);
    }
}