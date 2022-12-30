using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ABJAD.InterpretEngine.Service.Api;

public class InterpretationFailureFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is InterpretationException exception)
        {
            context.Result = new BadRequestObjectResult(new {exception.ArabicMessage, exception.EnglishMessage});
        }
    }
}