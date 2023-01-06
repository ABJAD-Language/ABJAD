using ABJAD.InterpretEngine.Declarations;
using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Statements.Strategies;

namespace ABJAD.InterpretEngine.Statements;

public class StatementInterpreter : IStatementInterpreter
{
    private readonly ScopeFacade scope;
    private readonly IExpressionEvaluator expressionEvaluator;
    private readonly DeclarationInterpreter declarationInterpreter;
    private readonly TextWriter writer;

    public StatementInterpreter(ScopeFacade scope, TextWriter writer)
    {
        this.scope = scope;
        this.writer = writer;
        expressionEvaluator = new ExpressionEvaluator(scope, writer);
        declarationInterpreter = new DeclarationInterpreter(scope, writer);
    }

    public StatementInterpretationResult Interpret(Statement target, bool functionContext = false)
    {
        scope.AddNewScope();
        var result = GetStrategy(target, functionContext).Apply();
        scope.RemoveLastScope();

        return result;
    }

    private StatementInterpretationStrategy GetStrategy(Statement target, bool functionContext)
    {
        return target switch
        {
            ExpressionStatement expressionStatement => new ExpressionStatementInterpretationStrategy(expressionStatement, expressionEvaluator),
            Assignment assignment => new AssignmentInterpretationStrategy(assignment, scope, expressionEvaluator),
            Block block => HandleBlock(block, functionContext),
            ForLoop forLoop => new ForLoopInterpretationStrategy(forLoop, functionContext,this, declarationInterpreter, expressionEvaluator),
            WhileLoop whileLoop => new WhileLoopInterpretationStrategy(whileLoop, functionContext, expressionEvaluator, this),
            Print print => new PrintInterpretationStrategy(print, writer, expressionEvaluator),
            IfElse ifElse => new IfElseInterpretationStrategy(ifElse, functionContext, this, expressionEvaluator),
            Return @return => new ReturnInterpretationStrategy(@return, functionContext, expressionEvaluator),
            _ => throw new ArgumentException()
        };
    }

    private StatementInterpretationStrategy HandleBlock(Block block, bool functionContext)
    {
        var statementInterpreter = new StatementInterpreter(scope, writer);
        var declarationInterpreter = new DeclarationInterpreter(scope, writer);
        return new BlockInterpretationStrategy(block, functionContext, statementInterpreter, declarationInterpreter);
    }
}