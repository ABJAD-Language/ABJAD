using ABJAD.ParseEngine.Shared;
using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine;

public class TokenConsumer
{
    private readonly List<Token> tokens;
    private int headIndex;

    public TokenConsumer(List<Token> tokens, int headIndex)
    {
        Guard.Against.NullOrEmpty(tokens);
        Guard.Against.Negative(headIndex);
        
        this.tokens = tokens;
        this.headIndex = headIndex;
    }

    public Token Consume(TokenType targetType)
    {
        GuardAgainstIndexOutOfRange(targetType);
        SkipWhiteSpaces();
        
        var headToken = GetHeadToken();
        if (headToken.Type == targetType)
        {
            MoveIndexForward();
            return headToken;
        }

        throw new ExpectedTokenNotFoundException(headToken.Line, headToken.Index, targetType);
    }

    private void GuardAgainstIndexOutOfRange(TokenType targetType)
    {
        if (tokens.Count <= headIndex)
        {
            var previousToken = tokens[headIndex - 1];
            throw new ExpectedTokenNotFoundException(previousToken.Line, previousToken.Index + 1, targetType);
        }
    }

    private void SkipWhiteSpaces()
    {
        while (GetHeadToken().Type == TokenType.WHITE_SPACE)
        {
            MoveIndexForward();
        }
    }

    private void MoveIndexForward()
    {
        headIndex++;
    }

    private Token GetHeadToken()
    {
        return tokens[headIndex];
    }
}