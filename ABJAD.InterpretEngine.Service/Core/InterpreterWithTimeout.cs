using ABJAD.InterpretEngine.Declarations;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Service.Api;
using ABJAD.InterpretEngine.Service.Mappers;
using ABJAD.InterpretEngine.Shared;
using ABJAD.InterpretEngine.Statements;

namespace ABJAD.InterpretEngine.Service.Core;

public class InterpreterWithTimeout : InterpreterService
{
    private const int TimeoutInSeconds = 20;

    public string Interpret(List<object> target)
    {
        var task = Task.Run(() => InterpretBindings(target));
        if (task.Wait(TimeSpan.FromSeconds(TimeoutInSeconds)))
            return task.Result;
        
        throw new TimeOutException();
    }

    private static string InterpretBindings(List<object> target)
    {
        var bindings = target.Select(MapBinding).ToList();

        var environment = EnvironmentFactory.NewEnvironment();
        var writer = new StringWriter();
        var statementInterpreter = new StatementInterpreter(environment, writer);
        var declarationInterpreter = new DeclarationInterpreter(environment, writer);
        var interpreter = new BindingInterpreter(statementInterpreter, declarationInterpreter);

        interpreter.Interpret(bindings);

        return writer.ToString();
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