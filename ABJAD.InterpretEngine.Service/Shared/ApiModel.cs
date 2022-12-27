using System.Text.Json.Serialization;

namespace ABJAD.InterpretEngine.Service.Shared;

public class ApiModel
{
    [JsonPropertyName(("_type"))] 
    public string Type { get; set; } = null!;

    public ApiModel()
    {
        
    }
}