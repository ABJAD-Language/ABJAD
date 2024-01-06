using ABJAD.Parser.Bindings;
using ABJAD.Parser.Domain;
using ABJAD.Parser.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace ABJAD.Parser.Api;

[ApiController]
[Route("/parser/")]
public class ParserController : ControllerBase
{
    [HttpPost]
    [ParsingFailureFilter]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public ActionResult<List<BindingApiModel>> Parse([FromBody] ParseTokensRequest request)
    {
        var parser = new ParserService(request.Tokens.Select(Map).ToList());
        var bindings = parser.Parse().Select(BindingApiModelMapper.Map).ToList();

        return Content(Serialize(bindings), "application/json");
    }

    private static string Serialize(List<BindingApiModel> bindings)
    {
        return JsonConvert.SerializeObject(bindings, new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        });
    }

    private static Token Map(TokenApiModel t)
    {
        return new Token { Content = t.Content, Index = t.Index, Line = t.Line, Type = t.Type };
    }
}