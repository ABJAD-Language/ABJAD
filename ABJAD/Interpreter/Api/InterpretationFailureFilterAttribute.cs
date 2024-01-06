using ABJAD.Interpreter.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ABJAD.Interpreter.Api;

public class InterpretationFailureFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is InterpretationException exception)
        {
            context.Result = new BadRequestObjectResult(new { exception.ArabicMessage, exception.EnglishMessage });
        }
    }
}