using ABJAD.Parser.Domain.Types;

namespace ABJAD.Parser.Domain.Declarations;

public interface ITypeConsumer
{
    DataType Consume();
    DataType ConsumeTypeOrVoid();
}