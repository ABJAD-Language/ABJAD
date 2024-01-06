using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;

namespace ABJAD.Test.Parser.Api;

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
        var request = ReadFile("Parser/Api/Requests/for_statement.json");
        var body = new StringContent(request, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync("/parser/", body);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(RemoveWhiteSpaces(ReadFile("Parser/Api/Responses/for_statement.json")), content);
    }

    [Fact(DisplayName = "parsing function declaration returns correct result")]
    public async Task parsing_function_declaration_returns_correct_result()
    {
        var request = ReadFile("Parser/Api/Requests/function_declaration.json");
        var body = new StringContent(request, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync("/parser/", body);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(RemoveWhiteSpaces(ReadFile("Parser/Api/Responses/function_declaration.json")), content);
    }

    [Fact(DisplayName = "parsing class declaration returns correct result")]
    public async Task parsing_class_declaration_returns_correct_result()
    {
        var request = ReadFile("Parser/Api/Requests/class_declaration.json");
        var body = new StringContent(request, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync("/parser/", body);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(RemoveWhiteSpaces(ReadFile("Parser/Api/Responses/class_declaration.json")), content);
    }

    [Fact(DisplayName = "parsing if else statament")]
    public async Task parsing_if_else_statament()
    {
        var request =
            @"{""tokens"":[{""line"":1,""index"":1,""type"":""IF"",""content"":""اذا""},{""line"":1,""index"":4,""type"":""WHITE_SPACE"",""content"":"" ""},{""line"":1,""index"":5,""type"":""OPEN_PAREN"",""content"":""(""},{""line"":1,""index"":6,""type"":""ID"",""content"":""ب""},{""line"":1,""index"":7,""type"":""WHITE_SPACE"",""content"":"" ""},{""line"":1,""index"":8,""type"":""EQUAL_EQUAL"",""content"":""==""},{""line"":1,""index"":10,""type"":""WHITE_SPACE"",""content"":"" ""},{""line"":1,""index"":11,""type"":""NUMBER_CONST"",""content"":""1""},{""line"":1,""index"":12,""type"":""CLOSE_PAREN"",""content"":"")""},{""line"":1,""index"":13,""type"":""WHITE_SPACE"",""content"":"" ""},{""line"":1,""index"":14,""type"":""OPEN_BRACE"",""content"":""{""},{""line"":1,""index"":15,""type"":""WHITE_SPACE"",""content"":""\n    ""},{""line"":2,""index"":1,""type"":""PRINT"",""content"":""اكتب""},{""line"":2,""index"":5,""type"":""WHITE_SPACE"",""content"":"" ""},{""line"":2,""index"":6,""type"":""OPEN_PAREN"",""content"":""(""},{""line"":2,""index"":7,""type"":""STRING_CONST"",""content"":"" مزدوج""},{""line"":2,""index"":15,""type"":""CLOSE_PAREN"",""content"":"")""},{""line"":2,""index"":16,""type"":""SEMICOLON"",""content"":""؛""},{""line"":2,""index"":17,""type"":""WHITE_SPACE"",""content"":""\n""},{""line"":3,""index"":1,""type"":""CLOSE_BRACE"",""content"":""}""},{""line"":3,""index"":2,""type"":""WHITE_SPACE"",""content"":"" ""},{""line"":3,""index"":3,""type"":""ELSE"",""content"":""والا""},{""line"":3,""index"":7,""type"":""WHITE_SPACE"",""content"":"" ""},{""line"":3,""index"":8,""type"":""IF"",""content"":""اذا""},{""line"":3,""index"":11,""type"":""WHITE_SPACE"",""content"":"" ""},{""line"":3,""index"":12,""type"":""OPEN_PAREN"",""content"":""(""},{""line"":3,""index"":13,""type"":""ID"",""content"":""ب""},{""line"":3,""index"":14,""type"":""WHITE_SPACE"",""content"":"" ""},{""line"":3,""index"":15,""type"":""EQUAL_EQUAL"",""content"":""==""},{""line"":3,""index"":17,""type"":""WHITE_SPACE"",""content"":"" ""},{""line"":3,""index"":18,""type"":""NUMBER_CONST"",""content"":""0""},{""line"":3,""index"":19,""type"":""CLOSE_PAREN"",""content"":"")""},{""line"":3,""index"":20,""type"":""WHITE_SPACE"",""content"":"" ""},{""line"":3,""index"":21,""type"":""OPEN_BRACE"",""content"":""{""},{""line"":3,""index"":22,""type"":""WHITE_SPACE"",""content"":""\n    ""},{""line"":4,""index"":1,""type"":""PRINT"",""content"":""اكتب""},{""line"":4,""index"":5,""type"":""WHITE_SPACE"",""content"":"" ""},{""line"":4,""index"":6,""type"":""OPEN_PAREN"",""content"":""(""},{""line"":4,""index"":7,""type"":""STRING_CONST"",""content"":"" مزدوج""},{""line"":4,""index"":15,""type"":""CLOSE_PAREN"",""content"":"")""},{""line"":4,""index"":16,""type"":""SEMICOLON"",""content"":""؛""},{""line"":4,""index"":17,""type"":""WHITE_SPACE"",""content"":""\n""},{""line"":5,""index"":1,""type"":""CLOSE_BRACE"",""content"":""}""},{""line"":5,""index"":2,""type"":""WHITE_SPACE"",""content"":"" ""},{""line"":5,""index"":3,""type"":""ELSE"",""content"":""والا""},{""line"":5,""index"":7,""type"":""WHITE_SPACE"",""content"":"" ""},{""line"":5,""index"":8,""type"":""OPEN_BRACE"",""content"":""{""},{""line"":5,""index"":9,""type"":""WHITE_SPACE"",""content"":""\n    ""},{""line"":6,""index"":1,""type"":""PRINT"",""content"":""اكتب""},{""line"":6,""index"":5,""type"":""WHITE_SPACE"",""content"":"" ""},{""line"":6,""index"":6,""type"":""OPEN_PAREN"",""content"":""(""},{""line"":6,""index"":7,""type"":""STRING_CONST"",""content"":""قبيح""},{""line"":6,""index"":13,""type"":""CLOSE_PAREN"",""content"":"")""},{""line"":6,""index"":14,""type"":""SEMICOLON"",""content"":""؛""},{""line"":6,""index"":15,""type"":""WHITE_SPACE"",""content"":""\n""},{""line"":7,""index"":1,""type"":""CLOSE_BRACE"",""content"":""}""}]}";
        var body = new StringContent(request, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync("/parser/", body);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private static string ReadFile(string fileName)
    {
        return File.ReadAllText("../../../" + fileName);
    }

    private static string RemoveWhiteSpaces(string content)
    {
        return Regex.Replace(content, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
    }
}