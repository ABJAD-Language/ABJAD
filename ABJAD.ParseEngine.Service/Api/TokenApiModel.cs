using System.Text.Json.Serialization;
using ABJAD.ParseEngine.Shared;

namespace ABJAD.ParseEngine.Service.Api;

public class TokenApiModel
{
    public int Line { get; set; }
    public int Index { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TokenType Type { get; set; }
    public string Content { get; set; }
}