using ABJAD.Interpreter.Shared;
using System.Text.Json;

namespace ABJAD.Interpreter;

public static class JsonUtils
{
    public static T Deserialize<T>(object jsonObject)
    {
        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        var result = JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(jsonObject), jsonSerializerOptions);

        if (result == null)
        {
            throw new ArgumentException();
        }

        return result;
    }

    public static string GetType(object jsonObject)
    {
        return Deserialize<ApiModel>(jsonObject)!.Type;
    }
}