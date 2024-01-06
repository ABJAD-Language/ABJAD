using ABJAD.Parser.Domain.Shared;
using System.Text.Json.Serialization;

namespace ABJAD.Parser.Api;

public class TokenApiModel
{
    public int Line { get; set; }
    public int Index { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TokenType Type { get; set; }
    public string Content { get; set; }
}