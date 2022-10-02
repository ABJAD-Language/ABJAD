namespace ABJAD.ParseEngine.Declarations;

public interface IParameterListConsumer
{
    List<FunctionParameter> Consume();
}