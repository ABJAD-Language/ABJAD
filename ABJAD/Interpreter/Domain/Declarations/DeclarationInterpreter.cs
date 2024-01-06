using ABJAD.Interpreter.Domain.Declarations.Strategies;
using ABJAD.Interpreter.Domain.Expressions;
using ABJAD.Interpreter.Domain.ScopeManagement;
using ABJAD.Interpreter.Domain.Shared.Declarations;

namespace ABJAD.Interpreter.Domain.Declarations;

public class DeclarationInterpreter : IDeclarationInterpreter
{
    private readonly ScopeFacade scope;
    private readonly IExpressionEvaluator expressionEvaluator;

    public DeclarationInterpreter(ScopeFacade scope, TextWriter writer)
    {
        this.scope = scope;
        expressionEvaluator = new ExpressionEvaluator(scope, writer);
    }

    public void Interpret(Declaration target)
    {
        GetStrategy(target).Apply();
    }

    private DeclarationInterpretationStrategy GetStrategy(Declaration target)
    {
        return target switch
        {
            VariableDeclaration variableDeclaration => new VariableDeclarationInterpretationStrategy(variableDeclaration, scope, expressionEvaluator),
            ConstantDeclaration constantDeclaration => new ConstantDeclarationInterpretationStrategy(constantDeclaration, scope),
            ClassDeclaration classDeclaration => new ClassDeclarationInterpretationStrategy(classDeclaration, scope),
            FunctionDeclaration functionDeclaration => new FunctionDeclarationInterpretationStrategy(functionDeclaration, scope),
            _ => throw new ArgumentException()
        };
    }
}