using ABJAD.Lexer.Domain.Tokens;

namespace ABJAD.Lexer.Domain;

public interface ILexer
{
    List<Token> Lex(string code);
}