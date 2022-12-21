using ABJAD.InterpretEngine.Shared;
using ABJAD.InterpretEngine.Shared.Declarations;
using ABJAD.InterpretEngine.Shared.Statements;

namespace ABJAD.InterpretEngine.Statements.Strategies;

public class BlockInterpretationStrategy : StatementInterpretationStrategy
{
    private readonly Block block;
    private readonly Interpreter<Binding> bindingInterpreter;

    public BlockInterpretationStrategy(Block block, Interpreter<Statement> statementInterpreter, Interpreter<Declaration> declarationInterpreter)
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