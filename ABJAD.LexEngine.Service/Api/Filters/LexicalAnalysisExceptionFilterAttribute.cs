using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ABJAD.LexEngine.Service.Api.Filters;

public class LexicalAnalysisExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is LexicalAnalysisException exception)
        {
            context.Result = new BadRequestObjectResult(new {exception.ArabicMessage, exception.EnglishMessage});
        }
    }
}