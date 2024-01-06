namespace ABJAD.Parser.Domain.Shared;

public enum TokenType
{
    /* Operations */
    AND,
    BANG,
    BANG_EQUAL,
    SLASH,
    SLASH_EQUAL,
    EQUAL,
    EQUAL_EQUAL,
    GREATER_EQUAL,
    GREATER_THAN,
    LESS_EQUAL,
    LESS_THAN,
    DASH,
    DASH_DASH,
    DASH_EQUAL,
    MODULO,
    OR,
    PLUS,
    PLUS_PLUS,
    PLUS_EQUAL,
    STAR,
    STAR_EQUAL,

    /* Other graphic characters */
    CLOSE_BRACE,
    CLOSE_PAREN,
    COMMA,
    DOT,
    OPEN_BRACE,
    OPEN_PAREN,
    SEMICOLON,
    COLON,

    /* Keywords */
    BOOL,
    CLASS,
    CONST,
    CONSTRUCTOR,
    ELSE,
    FALSE,
    FOR,
    FUNC,
    IF,
    NEW,
    NULL,
    NUMBER,
    PRINT,
    RETURN,
    STRING,
    TYPEOF,
    TRUE,
    WHILE,
    VAR,
    VOID,

    /* Primitives */
    NUMBER_CONST,
    ID,
    STRING_CONST,

    WHITE_SPACE,
    COMMENT,
}