using LexEngine.Tokens;

namespace LexEngine;

public interface ILexer
{
    List<Token> Lex(string code);
}