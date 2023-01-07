using System.Net;
using ABJAD.InterpretEngine.Service.Core;
using Microsoft.AspNetCore.Mvc;

namespace ABJAD.InterpretEngine.Service.Api;


[ApiController]
[Route("/")]
public class InterpretController : ControllerBase
{
    private readonly InterpreterService interpreterService;

    public InterpretController(InterpreterService interpreterService)
    {
        this.interpreterService = interpreterService;
    }

    [HttpPost]
    [InterpretationFailureFilter]
    [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(List<InterpretBindingResponse>))]
    public ActionResult<InterpretBindingResponse> Interpret([FromBody] InterpretBindingsRequest request)
    {
        var output = interpreterService.Interpret(request.bindings);
        return Ok(new InterpretBindingResponse { Output = output });
    }

}