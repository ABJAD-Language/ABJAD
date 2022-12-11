using Newtonsoft.Json;

namespace ABJAD.ParseEngine.Service;

public abstract class ApiModel
{
    [JsonProperty("_type")]
    public string Type { get; init; } = null!;
}