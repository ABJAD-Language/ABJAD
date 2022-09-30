using ABJAD.ParseEngine.Shared;

namespace ABJAD.ParseEngine;

public interface ITokenConsumer
{
    Token Consume(TokenType targetType);
    Token Consume();

    Token Peek();
    bool CanConsume();
}