using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;

namespace ABJAD.Test.Lexer.Api;

public class LexicalAnalysisControllerTest
{
    private readonly HttpClient client;

    public LexicalAnalysisControllerTest()
    {
        var appFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {

            });

        client = appFactory.CreateClient();
    }

    [Fact]
    public async Task LexicalAnalysisReturnsExpectedTokensOnHappyPath()
    {
        var request = ReadFile("lexical_analysis_request.json");
        var body = new StringContent(request, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync("/lexer/", body);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal(RemoveWhiteSpaces(ReadFile("lexical_analysis_response.json")), result);
    }

    [Fact]
    public async Task LexicalAnalysisReturnsBadRequestWithErrorMessageWhenFailure()
    {
        var request = ReadFile("lexical_analysis_failure_request.json");
        var body = new StringContent(request, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync("/lexer/", body);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal(RemoveWhiteSpaces(ReadFile("lexical_analysis_failure_response.json")), result);
    }

    private static string ReadFile(string fileName)
    {
        return File.ReadAllText("../../../Lexer/Api/" + fileName);
    }

    private static string RemoveWhiteSpaces(string content)
    {
        return Regex.Replace(content, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
    }
}