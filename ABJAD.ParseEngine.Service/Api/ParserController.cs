using System.Net;
using System.Text.Json;
using ABJAD.ParseEngine.Service.Bindings;
using ABJAD.ParseEngine.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ABJAD.ParseEngine.Service.Api;

[ApiController]
[Route("/")]
public class ParserController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    public ActionResult<List<BindingApiModel>> Parse([FromBody] ParseTokensRequest request)
    {
        var parser = new Parser(request.Tokens.Select(Map).ToList());
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
        return new Token { Content = t.Content, Index = t.Index, Line = t.Line, Type = t.Type};
    }
}