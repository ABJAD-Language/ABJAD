using ABJAD.InterpretEngine.Declarations;
using ABJAD.InterpretEngine.Shared.Statements;

namespace ABJAD.InterpretEngine.Statements.Strategies;

public class BlockInterpretationStrategy : StatementInterpretationStrategy
{
    private readonly Block block;
    private readonly Interpreter bindingInterpreter;

    public BlockInterpretationStrategy(Block block, IStatementInterpreter statementInterpreter, IDeclarationInterpreter declarationInterpreter)
    {
        this.block = block;
        bindingInterpreter = new BindingInterpreter(statementInterpreter, declarationInterpreter);
    }

    public void Apply()
    {
        foreach (var binding in block.Bindings)
        {
            bindingInterpreter.Interpret(binding);
        }
    }
}