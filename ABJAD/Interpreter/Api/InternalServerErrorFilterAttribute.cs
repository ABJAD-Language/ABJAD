using ABJAD.Interpreter.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ABJAD.Interpreter.Api;

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