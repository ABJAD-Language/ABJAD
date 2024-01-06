using ABJAD.Interpreter.Domain.Types;

namespace ABJAD.Interpreter.Domain.ScopeManagement;

public record StateElement
{
    public DataType Type { get; init; }
    public object Value { get; init; }
    public bool IsConstant { get; set; }
}