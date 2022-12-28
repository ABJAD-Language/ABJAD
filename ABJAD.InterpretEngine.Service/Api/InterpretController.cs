using ABJAD.InterpretEngine.Declarations;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Service.Mappers;
using ABJAD.InterpretEngine.Shared;
using ABJAD.InterpretEngine.Statements;
using Microsoft.AspNetCore.Mvc;

namespace ABJAD.InterpretEngine.Service.Api;

[ApiController]
[Route("/")]
public class InterpretController : ControllerBase
{
    [HttpPost]
    public ActionResult<string> Interpret([FromBody] InterpretBindingsRequest request)
    {
        var bindings = request.bindings.Select(MapBinding).ToList();

        var environment = EnvironmentFactory.NewEnvironment();
        var writer = new StringWriter();
        var statementInterpreter = new StatementInterpreter(environment, writer);
        var declarationInterpreter = new DeclarationInterpreter(environment, writer);
        var interpreter = new BindingInterpreter(statementInterpreter, declarationInterpreter);

        interpreter.Interpret(bindings);

        return Ok(writer.ToString());
    }

    private static Binding MapBinding(object requestBinding)
    {
        var type = JsonUtils.GetType(requestBinding).Split(".")[0];
        return type switch
        {
            "declaration" => DeclarationMapper.Map(requestBinding),
            "statement" => StatementMapper.Map(requestBinding)
        };
    }
}