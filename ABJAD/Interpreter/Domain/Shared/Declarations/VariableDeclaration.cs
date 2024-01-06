using ABJAD.Interpreter.Domain.Shared.Expressions;
using ABJAD.Interpreter.Domain.Types;

namespace ABJAD.Interpreter.Domain.Shared.Declarations;

public class VariableDeclaration : Declaration
{
    public string Name { get; set; }
    public DataType Type { get; set; }
    public Expression? Value { get; set; }
}