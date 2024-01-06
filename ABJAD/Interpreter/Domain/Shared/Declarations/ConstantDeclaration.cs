using ABJAD.Interpreter.Domain.Shared.Expressions.Primitives;
using ABJAD.Interpreter.Domain.Types;

namespace ABJAD.Interpreter.Domain.Shared.Declarations;

public class ConstantDeclaration : Declaration
{
    public string Name { get; set; }
    public DataType Type { get; set; }
    public Primitive Value { get; set; }
}