using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ABJAD.ParseEngine.Service.Api;

public class ParsingFailureFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is ParsingException exception)
        {
            context.Result = new BadRequestObjectResult(new { exception.ArabicMessage, exception.EnglishMessage });
        }
    }
}