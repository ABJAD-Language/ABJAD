using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Declarations;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.Expressions.Strategies;

public class MethodCallEvaluationStrategy : ExpressionEvaluationStrategy
{
    private readonly MethodCall methodCall;
    private readonly ScopeFacade scope;
    private readonly Evaluator<Expression> expressionEvaluator;
    private readonly Interpreter<Statement> statementInterpreter;
    private readonly Interpreter<Declaration> declarationInterpreter;

    public MethodCallEvaluationStrategy(MethodCall methodCall, ScopeFacade scope, Evaluator<Expression> expressionEvaluator, Interpreter<Statement> statementInterpreter, Interpreter<Declaration> declarationInterpreter)
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

        return InterpretBody(function);
    }

    private EvaluatedResult InterpretBody(FunctionElement function)
    {
        var result = new EvaluatedResult() { Type = DataType.Undefined(), Value = SpecialValues.UNDEFINED };
        foreach (var binding in function.Body.Bindings)
        {
            if (binding is Declaration declaration)
            {
                declarationInterpreter.Interpret(declaration);
            }
            else if (binding is Statement statement)
            {
                if (statement is Return returnStatement)
                {
                    result = HandleReturnStatement(function, returnStatement);
                    break;
                }

                statementInterpreter.Interpret(statement);
            }
        }

        return result;
    }

    private EvaluatedResult HandleReturnStatement(FunctionElement function, Return returnStatement)
    {
        if (function.ReturnType == null)
        {
            if (returnStatement.Target != null)
            {
                throw new InvalidVoidReturnStatementException(methodCall.MethodName);
            }

            return new EvaluatedResult() { Type = DataType.Undefined(), Value = SpecialValues.UNDEFINED };
        }

        return EvaluateReturnTarget(function, returnStatement);
    }

    private EvaluatedResult EvaluateReturnTarget(FunctionElement function, Return returnStatement)
    {
        var returnTarget = expressionEvaluator.Evaluate(returnStatement.Target);

        if (!returnTarget.Type.Is(function.ReturnType))
        {
            throw new InvalidTypeException(returnTarget.Type, function.ReturnType);
        }

        return returnTarget;
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