using Newtonsoft.Json;

namespace ABJAD.ParseEngine.Service.Declarations;

public class FunctionParameterApiModel
{
    public string Name { get; set; }
    [JsonProperty("type")]
    public string ParameterType { get; set; }
}