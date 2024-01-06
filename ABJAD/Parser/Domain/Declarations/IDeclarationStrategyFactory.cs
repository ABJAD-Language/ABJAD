using ABJAD.Parser.Domain.Shared;

namespace ABJAD.Parser.Domain.Declarations;

public interface IDeclarationStrategyFactory
{
    public static List<TokenType> DeclarationTokenTypes = new()
        { TokenType.VAR, TokenType.CONST, TokenType.FUNC, TokenType.CONSTRUCTOR, TokenType.CLASS };

    ParseDeclarationStrategy Get();
}