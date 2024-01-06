using ABJAD.Interpreter.Core;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ABJAD.Interpreter.Api;


[ApiController]
[Route("/interpreter/")]
public class InterpretController : ControllerBase
{
    private readonly InterpreterService interpreterService;

    public InterpretController(InterpreterService interpreterService)
    {
        this.interpreterService = interpreterService;
    }

    [HttpPost]
    [InterpretationFailureFilter]
    [InternalServerErrorFilter]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<InterpretBindingResponse>))]
    public ActionResult<InterpretBindingResponse> Interpret([FromBody] InterpretBindingsRequest request)
    {
        var output = interpreterService.Interpret(request.bindings);
        return Ok(new InterpretBindingResponse { Output = output });
    }

}