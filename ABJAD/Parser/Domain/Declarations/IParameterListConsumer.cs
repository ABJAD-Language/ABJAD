namespace ABJAD.Parser.Domain.Declarations;

public interface IParameterListConsumer
{
    List<FunctionParameter> Consume();
}