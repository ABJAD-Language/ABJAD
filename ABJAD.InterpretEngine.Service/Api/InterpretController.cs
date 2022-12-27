using ABJAD.InterpretEngine.Service.Mappers;
using ABJAD.InterpretEngine.Shared.Expressions.Primitives;
using Microsoft.AspNetCore.Mvc;

namespace ABJAD.InterpretEngine.Service.Api;

[ApiController]
[Route("/")]
public class InterpretController : ControllerBase
{
    [HttpPost]
    public ActionResult<string> Interpret([FromBody] InterpretBindingsRequest request)
    {
        foreach (var requestBinding in request.bindings)
        {
            if (JsonUtils.GetType(requestBinding) == "expression.primitive.number")
            {
                var expression = ExpressionMapper.Map(requestBinding);
                Console.WriteLine(((NumberPrimitive)expression).Value);
            }
        }

        return null;
    }
}