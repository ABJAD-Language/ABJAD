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

    [Fact(DisplayName = "parsing for statement returns correct result")]
    public async Task parsing_for_statement_returns_correct_result()
    {
        var request = ReadFile("Requests/for_statement.json");
        var body = new StringContent(request, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync("/", body);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(RemoveWhiteSpaces(ReadFile("Responses/for_statement.json")), content);
    }

    [Fact(DisplayName = "parsing function declaration returns correct result")]
    public async Task parsing_function_declaration_returns_correct_result()
    {
        var request = ReadFile("Requests/function_declaration.json");
        var body = new StringContent(request, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync("/", body);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(RemoveWhiteSpaces(ReadFile("Responses/function_declaration.json")), content);
    }

    [Fact(DisplayName = "parsing class declaration returns correct result")]
    public async Task parsing_class_declaration_returns_correct_result()
    {
        var request = ReadFile("Requests/class_declaration.json");
        var body = new StringContent(request, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync("/", body);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(RemoveWhiteSpaces(ReadFile("Responses/class_declaration.json")), content);
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