using ABJAD.InterpretEngine.Declarations;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Declarations;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Statements;
using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.Expressions.Strategies;

public class MethodCallEvaluationStrategy : ExpressionEvaluationStrategy
{
    private readonly MethodCall methodCall;
    private readonly ScopeFacade scope;
    private readonly IExpressionEvaluator expressionEvaluator;
    private readonly IStatementInterpreter statementInterpreter;
    private readonly IDeclarationInterpreter declarationInterpreter;

    public MethodCallEvaluationStrategy(MethodCall methodCall, ScopeFacade scope, IExpressionEvaluator expressionEvaluator, IStatementInterpreter statementInterpreter, IDeclarationInterpreter declarationInterpreter)
    {
        this.methodCall = methodCall;
        this.scope = scope;
        this.expressionEvaluator = expressionEvaluator;
        this.statementInterpreter = statementInterpreter;
        this.declarationInterpreter = declarationInterpreter;
    }

    public EvaluatedResult Apply()
    {
        var args = EvaluateArguments();
        
        var argsTypes = args.Select(arg => arg.Type).ToArray();
        ValidateMethodExists(argsTypes);

        var function = scope.GetFunction(methodCall.MethodName, argsTypes);

        AddArgumentsToScope(function.Parameters, args);

        var evaluatedResult = InterpretBody(function);
        
        RemoveArgumentsValuesFromScope();

        return evaluatedResult;
    }

    private void RemoveArgumentsValuesFromScope()
    {
        scope.RemoveLastScope();
    }

    private EvaluatedResult InterpretBody(FunctionElement function)
    {
        EvaluatedResult? result = null;
        foreach (var binding in function.Body.Bindings)
        {
            if (binding is Declaration declaration)
            {
                declarationInterpreter.Interpret(declaration);
            }
            else if (binding is Statement statement)
            {
                var interpretationResult = statementInterpreter.Interpret(statement, true);
                if (interpretationResult.Returned)
                {
                    result = interpretationResult.ReturnedValue;
                    break;
                }
            }
        }

        return ValidateReturnType(function, result);
    }

    private EvaluatedResult ValidateReturnType(FunctionElement function, EvaluatedResult? result)
    {
        if (function.ReturnType == null)
        {
            if (result != null)
            {
                throw new InvalidVoidReturnStatementException(methodCall.MethodName);
            }

            return new EvaluatedResult() { Type = DataType.Undefined(), Value = SpecialValues.UNDEFINED };
        }

        if (result == null)
        {
            throw new NoValueReturnedException(methodCall.MethodName, function.ReturnType);
        }

        ValidateTypesMatch(function, result);

        if (IsValueNull(result))
        {
            result.Type = function.ReturnType;
        }

        return result;
    }

    private static void ValidateTypesMatch(FunctionElement function, EvaluatedResult result)
    {
        if (!function.ReturnType!.Is(result.Type) && !IsValueNull(result))
        {
            throw new InvalidTypeException(result.Type, function.ReturnType);
        }
    }

    private static bool IsValueNull(EvaluatedResult result)
    {
        return result.Type.IsUndefined() && result.Value.Equals(SpecialValues.NULL);
    }

    private List<EvaluatedResult> EvaluateArguments()
    {
        var arguments = methodCall.Arguments.Select(arg => expressionEvaluator.Evaluate(arg)).ToList();
        ValidateArgumentsAreNotUndefined(arguments);
        return arguments;
    }

    private static void ValidateArgumentsAreNotUndefined(List<EvaluatedResult> args)
    {
        if (args.Any(arg => arg.Value.Equals(SpecialValues.UNDEFINED)))
        {
            throw new OperationOnUndefinedValueException();
        }
    }

    private void AddArgumentsToScope(List<FunctionParameter> functionParameters, List<EvaluatedResult> args)
    {
        scope.AddNewScope();
        for (var i = 0; i < functionParameters.Count; i++)
        {
            scope.DefineVariable(functionParameters[i].Name, functionParameters[i].Type, args[i].Value);
        }
    }

    private void ValidateMethodExists(DataType[] argsTypes)
    {
        if (!scope.FunctionExists(methodCall.MethodName, argsTypes))
        {
            throw new MethodNotFoundException(methodCall.MethodName, argsTypes);
        }
    }
}