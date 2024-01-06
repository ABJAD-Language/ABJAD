using ABJAD.Parser.Domain.Shared;
using Ardalis.GuardClauses;

namespace ABJAD.Parser.Domain;

public class TokenConsumer : ITokenConsumer
{
    private readonly List<Token> tokens;
    private int headIndex;

    public TokenConsumer(List<Token> tokens, int headIndex)
    {
        Guard.Against.NullOrEmpty(tokens);
        Guard.Against.Negative(headIndex);
        this.tokens = tokens.Where(t => t.Type != TokenType.WHITE_SPACE && t.Type != TokenType.COMMENT).ToList();
        Guard.Against.NullOrEmpty(this.tokens);

        this.headIndex = headIndex;
    }

    public Token Consume(TokenType targetType)
    {
        GuardAgainstIndexOutOfRange();

        var headToken = GetHeadToken();
        if (headToken.Type == targetType)
        {
            MoveIndexForward();
            return headToken;
        }

        throw new ExpectedTokenNotFoundException(headToken.Line, headToken.Index, targetType);
    }

    public Token Consume()
    {
        GuardAgainstIndexOutOfRange();
        var headToken = GetHeadToken();
        MoveIndexForward();
        return headToken;
    }

    public Token Peek()
    {
        return GetHeadToken();
    }

    public Token LookAhead(int offset)
    {
        Guard.Against.Negative(offset);
        return tokens[headIndex + offset];
    }

    public bool CanConsume(TokenType targetType)
    {
        return CanConsume() && GetHeadToken().Type == targetType;
    }

    public bool CanConsume()
    {
        return tokens.Count > headIndex;
    }

    public int GetCurrentLine()
    {
        return GetHeadToken().Line;
    }

    public int GetCurrentIndex()
    {
        return GetHeadToken().Index;
    }

    private void GuardAgainstIndexOutOfRange()
    {
        if (tokens.Count <= headIndex)
        {
            throw new ArgumentOutOfRangeException();
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