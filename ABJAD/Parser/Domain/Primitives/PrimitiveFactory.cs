using ABJAD.Parser.Domain.Shared;

namespace ABJAD.Parser.Domain.Primitives;

public static class PrimitiveFactory
{
    public static Primitive Get(Token token)
    {
        return token.Type switch
        {
            TokenType.NUMBER_CONST => NumberPrimitive.From(token.Content),
            TokenType.STRING_CONST => StringPrimitive.From(token.Content),
            TokenType.TRUE => BoolPrimitive.True(),
            TokenType.FALSE => BoolPrimitive.False(),
            TokenType.NULL => NullPrimitive.Instance(),
            TokenType.ID => IdentifierPrimitive.From(token.Content),
            _ => throw new InvalidPrimitiveTypeException(token.Type, token.Line, token.Index)
        };
    }
}