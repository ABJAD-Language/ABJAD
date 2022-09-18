using ABJAD.LexEngine.Tokens;

namespace ABJAD.LexEngine;

public interface ILexer
{
    List<Token> Lex(string code);
}