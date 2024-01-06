using ABJAD.Lexer.Api.Filters;
using ABJAD.Lexer.Core;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ABJAD.Lexer.Api;

[ApiController]
[Route("/lexer/")]
public class LexicalAnalysisController : ControllerBase
{
    private readonly Analyzer analyzer;

    public LexicalAnalysisController(Analyzer analyzer)
    {
        this.analyzer = analyzer;
    }

    [HttpPost]
    [LexicalAnalysisExceptionFilter]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<LexicalToken>))]
    public ActionResult<List<LexicalToken>> ApplyLexicalAnalysis([FromBody] LexicalAnalysisRequest request)
    {
        var lexicalToken = analyzer.AnalyzeCode(request.Code);
        return Ok(lexicalToken);
    }
}