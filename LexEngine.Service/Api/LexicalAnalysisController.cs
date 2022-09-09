using System.Net;
using LexEngine.Service.Core;
using Microsoft.AspNetCore.Mvc;

namespace LexEngine.Service.Api;

[ApiController]
[Route("/")]
public class LexicalAnalysisController : ControllerBase
{
    private readonly Analyzer analyzer;

    public LexicalAnalysisController(Analyzer analyzer)
    {
        this.analyzer = analyzer;
    }

    [HttpPost]
    [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(List<LexicalToken>))]
    public ActionResult<List<LexicalToken>> ApplyLexicalAnalysis([FromBody] LexicalAnalysisRequest request)
    {
        var lexicalToken = analyzer.AnalyzeCode(request.Code);
        return Ok(lexicalToken);
    }
}