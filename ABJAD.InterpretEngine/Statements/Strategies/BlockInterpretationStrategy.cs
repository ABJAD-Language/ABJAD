using ABJAD.InterpretEngine.Declarations;
using ABJAD.InterpretEngine.Shared.Declarations;
using ABJAD.InterpretEngine.Shared.Statements;

namespace ABJAD.InterpretEngine.Statements.Strategies;

public class BlockInterpretationStrategy : StatementInterpretationStrategy
{
    private readonly Block block;
    private readonly bool functionContext;
    private readonly IStatementInterpreter statementInterpreter;
    private readonly IDeclarationInterpreter declarationInterpreter;

    public BlockInterpretationStrategy(Block block, bool functionContext, IStatementInterpreter statementInterpreter, IDeclarationInterpreter declarationInterpreter)
    {
        this.block = block;
        this.functionContext = functionContext;
        this.statementInterpreter = statementInterpreter;
        this.declarationInterpreter = declarationInterpreter;
    }

    public void Apply()
    {
        foreach (var binding in block.Bindings)
        {
            if (binding is Declaration declaration)
            {
                declarationInterpreter.Interpret(declaration);
            }
            else if (binding is Statement statement)
            {
                statementInterpreter.Interpret(statement, functionContext);  // TODO handle case of return statement
            }
        }
    }
}