namespace ABJAD.ParseEngine.Declarations;

public interface ITypeConsumer
{
    string Consume();
    string ConsumeTypeOrVoid();
}