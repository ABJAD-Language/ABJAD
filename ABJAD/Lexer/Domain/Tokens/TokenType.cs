namespace ABJAD.Lexer.Domain.Tokens;

public enum TokenType
{
    /* Operations */
    AND, BANG, BANG_EQUAL, SLASH, EQUAL, EQUAL_EQUAL, GREATER_EQUAL, GREATER_THAN, LESS_EQUAL, LESS_THAN, DASH, DASH_DASH,
    PLUS_EQUAL, DASH_EQUAL, STAR_EQUAL, SLASH_EQUAL, MODULO, OR, PLUS, PLUS_PLUS, STAR,

    /* Other graphic characters */
    CLOSE_BRACE, CLOSE_PAREN, COLON, COMMA, DOT, OPEN_BRACE, OPEN_PAREN, SEMICOLON,

    /* Keywords */
    BOOL, CLASS, CONST, ELSE, FALSE, FOR, FUNC, IF, NEW, NULL, NUMBER, PRINT, RETURN, STRING, TYPEOF, TRUE, WHILE, VAR,
    VOID, CONSTRUCTOR,

    /* Primitives */
    NUMBER_CONST, ID, STRING_CONST,

    WHITE_SPACE, COMMENT
}