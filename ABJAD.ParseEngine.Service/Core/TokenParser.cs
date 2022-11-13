using ABJAD.ParseEngine.Service.Api;
using ABJAD.ParseEngine.Shared;

namespace ABJAD.ParseEngine.Service.Core;

public class TokenParser
{
    private readonly ParserFactory parserFactory;
    private readonly IBindingApiMapper bindingApiMapper;

    public TokenParser(ParserFactory parserFactory, IBindingApiMapper bindingApiMapper)
    {
        this.parserFactory = parserFactory;
        this.bindingApiMapper = bindingApiMapper;
    }

    public List<BindingApiModel> Parse(List<Token> tokens)
    {
        var parser = parserFactory.Get(tokens);
        var bindings = parser.Parse();
        return bindings.Select(b => bindingApiMapper.Map(b)).ToList();
    }

}