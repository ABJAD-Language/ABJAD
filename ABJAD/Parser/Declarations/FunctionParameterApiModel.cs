using Newtonsoft.Json;

namespace ABJAD.Parser.Declarations;

public class FunctionParameterApiModel
{
    public string Name { get; set; }
    [JsonProperty("type")]
    public string ParameterType { get; set; }
}