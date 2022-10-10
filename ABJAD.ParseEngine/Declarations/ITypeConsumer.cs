
using ABJAD.ParseEngine.Types;

namespace ABJAD.ParseEngine.Declarations;

public interface ITypeConsumer
{
    DataType Consume();
    DataType ConsumeTypeOrVoid();
}