using Newtonsoft.Json;

namespace ABJAD.Parser;

public abstract class ApiModel
{
    [JsonProperty("_type")]
    public string Type { get; init; } = null!;
}