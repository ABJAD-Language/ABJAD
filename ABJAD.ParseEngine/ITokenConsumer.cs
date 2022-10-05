using ABJAD.ParseEngine.Shared;

namespace ABJAD.ParseEngine;

public interface ITokenConsumer
{
    Token Consume(TokenType targetType);
    Token Consume();
    Token Peek();
    Token LookAhead(int offset);
    bool CanConsume(TokenType targetType);
    bool CanConsume();
    int GetCurrentLine();
    int GetCurrentIndex();
}