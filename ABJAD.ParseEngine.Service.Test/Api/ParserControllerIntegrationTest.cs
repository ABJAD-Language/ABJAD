using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ABJAD.ParseEngine.Service.Test.Api;

public class ParserControllerIntegrationTest
{
    private readonly HttpClient client;

    public ParserControllerIntegrationTest()
    {
        var appFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(_ => { });
        client = appFactory.CreateClient();
    }

    [Fact(DisplayName = "parsing returns correct structure on happy path")]
    public async Task parsing_returns_correct_structure_on_happy_path()
    {
        var request = ReadFile("Requests/for_statement.json");
        var body = new StringContent(request, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync("/", body);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(RemoveWhiteSpaces(ReadFile("Responses/for_statement.json")), content);
    }
    
    private static string ReadFile(string fileName)
    {
        return File.ReadAllText("../../../Api/" + fileName);
    }

    private static string RemoveWhiteSpaces(string content)
    {
        return Regex.Replace(content, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
    }
}