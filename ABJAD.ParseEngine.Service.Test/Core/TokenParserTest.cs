using ABJAD.ParseEngine.Bindings;
using ABJAD.ParseEngine.Service.Api;
using ABJAD.ParseEngine.Service.Core;
using ABJAD.ParseEngine.Shared;
using Moq;

namespace ABJAD.ParseEngine.Service.Test.Core;

public class TokenParserTest
{
    private readonly TokenParser tokenParser;
    private readonly Mock<ParserFactory> parserFactoryMock = new();
    private readonly Mock<IParser> parserMock = new();
    private readonly Mock<IBindingApiMapper> bindingApiMapperMock = new();
    private readonly Mock<Binding> bindingMock = new();

    public TokenParserTest()
    {
        tokenParser = new TokenParser(parserFactoryMock.Object, bindingApiMapperMock.Object);
        parserFactoryMock.Setup(f => f.Get(It.IsAny<List<Token>>())).Returns(parserMock.Object);
        parserMock.Setup(p => p.Parse()).Returns(new List<Binding> {bindingMock.Object});
    }
    
    [Fact]
    public void generate_parser()
    {
        var tokens = new List<Token> { new() { Type = TokenType.SEMICOLON } };
        tokenParser.Parse(tokens);
        parserFactoryMock.Verify(f => f.Get(tokens));
    }

    [Fact(DisplayName = "parse tokens")]
    public void parse_tokens()
    {
        var tokens = new List<Token> { new() { Type = TokenType.SEMICOLON } };
        tokenParser.Parse(tokens);
        parserMock.Verify(p => p.Parse());
    }

    [Fact(DisplayName = "maps the bindings to api models")]
    public void maps_the_bindings_to_api_models()
    {
        var tokens = new List<Token> { new() { Type = TokenType.SEMICOLON } };
        tokenParser.Parse(tokens);
        bindingApiMapperMock.Verify(m => m.Map(bindingMock.Object));
    }


    [Fact(DisplayName = "returns the mapped list")]
    public void returns_the_mapped_list()
    {
        var bindingApiModel = new BindingApiModel();
        bindingApiMapperMock.Setup(m => m.Map(It.IsAny<Binding>())).Returns(bindingApiModel);
        var tokens = new List<Token> { new() { Type = TokenType.SEMICOLON } };
        var bindingApiModels = tokenParser.Parse(tokens);
        Assert.Equal(new List<BindingApiModel> {bindingApiModel}, bindingApiModels);
    }

}