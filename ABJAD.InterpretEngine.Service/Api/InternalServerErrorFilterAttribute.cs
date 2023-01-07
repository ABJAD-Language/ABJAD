using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ABJAD.InterpretEngine.Service.Api;

public class InternalServerErrorFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is not InterpretationException)
        {
            context.Result = new BadRequestObjectResult(new InterpretationFailureException());
        }
    }
}