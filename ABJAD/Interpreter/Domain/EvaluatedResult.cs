using ABJAD.Interpreter.Domain.Types;

namespace ABJAD.Interpreter.Domain;

public class EvaluatedResult
{
    public DataType Type { get; set; }
    public object Value { get; set; }
}