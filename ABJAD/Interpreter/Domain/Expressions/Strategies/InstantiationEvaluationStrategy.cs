using ABJAD.Interpreter.Domain.Declarations;
using ABJAD.Interpreter.Domain.ScopeManagement;
using ABJAD.Interpreter.Domain.Shared.Expressions;
using ABJAD.Interpreter.Domain.Statements;
using ABJAD.Interpreter.Domain.Types;

namespace ABJAD.Interpreter.Domain.Expressions.Strategies;

public class InstantiationEvaluationStrategy : ExpressionEvaluationStrategy
{
    private readonly Instantiation instantiation;
    private readonly ScopeFacade globalScope;
    private readonly ScopeFacade localScope;
    private readonly IExpressionEvaluator expressionEvaluator;
    private readonly IStatementInterpreter statementInterpreter;
    private readonly IDeclarationInterpreter declarationInterpreter;

    public InstantiationEvaluationStrategy(
        Instantiation instantiation,
        ScopeFacade globalScope,
        ScopeFacade localScope,
        IExpressionEvaluator expressionEvaluator,
        IStatementInterpreter statementInterpreter,
        IDeclarationInterpreter declarationInterpreter)
    {
        this.instantiation = instantiation;
        this.globalScope = globalScope;
        this.expressionEvaluator = expressionEvaluator;

        this.statementInterpreter = statementInterpreter;
        this.declarationInterpreter = declarationInterpreter;
        this.localScope = localScope;
    }

    public EvaluatedResult Apply()
    {
        ValidateClassExistsInScope();

        var arguments = EvaluateArguments();

        var constructorParameterTypes = arguments.Select(arg => arg.Type).ToArray();
        if (globalScope.TypeHasConstructor(instantiation.ClassName, constructorParameterTypes))
        {
            var classElement = globalScope.GetType(instantiation.ClassName);
            classElement.Declarations.ForEach(d => declarationInterpreter.Interpret(d));

            var constructorElement = globalScope.GetTypeConstructor(instantiation.ClassName, constructorParameterTypes);

            AddConstructorArgumentsToScope(constructorElement, arguments);

            statementInterpreter.Interpret(constructorElement.Body);

            RemoveArgumentsValuesFromScope();
        }
        else if (constructorParameterTypes.Length == 0)
        {
            var classElement = globalScope.GetType(instantiation.ClassName);
            classElement.Declarations.ForEach(d => declarationInterpreter.Interpret(d));
        }
        else
        {
            throw new NoSuitableConstructorFoundException(instantiation.ClassName, constructorParameterTypes);
        }

        return new EvaluatedResult
        {
            Type = DataType.Custom(instantiation.ClassName),
            Value = new InstanceElement { Scope = localScope }
        };
    }

    private void RemoveArgumentsValuesFromScope()
    {
        globalScope.RemoveLastScope();
    }

    private void AddConstructorArgumentsToScope(ConstructorElement constructorElement, List<EvaluatedResult> arguments)
    {
        globalScope.AddNewScope();
        for (int i = 0; i < constructorElement.Parameters.Count; i++)
        {
            globalScope.DefineVariable(constructorElement.Parameters[i].Name, constructorElement.Parameters[i].Type,
                arguments[i].Value);
        }
    }

    private List<EvaluatedResult> EvaluateArguments()
    {
        var arguments = instantiation.Arguments.Select(arg => expressionEvaluator.Evaluate(arg)).ToList();

        if (arguments.Any(a => a.Value.Equals(SpecialValues.UNDEFINED)))
        {
            throw new OperationOnUndefinedValueException();
        }

        return arguments;
    }

    private void ValidateClassExistsInScope()
    {
        if (!globalScope.TypeExists(instantiation.ClassName))
        {
            throw new ClassNotFoundException(instantiation.ClassName);
        }
    }
}