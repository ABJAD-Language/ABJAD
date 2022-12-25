using ABJAD.InterpretEngine.Declarations.Strategies;
using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Declarations;
using ABJAD.InterpretEngine.Shared.Expressions;

namespace ABJAD.InterpretEngine.Declarations;

public class DeclarationInterpreter : Interpreter<Declaration>
{
    private readonly ScopeFacade scope;
    private readonly Evaluator<Expression> expressionEvaluator;

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