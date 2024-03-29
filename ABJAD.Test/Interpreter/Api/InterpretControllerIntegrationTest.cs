﻿using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;

namespace ABJAD.Test.Interpreter.Api;

public class InterpretControllerIntegrationTest
{
    private readonly HttpClient client;

    public InterpretControllerIntegrationTest()
    {
        var applicationFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(_ => { });
        client = applicationFactory.CreateClient();
    }

    [Theory]
    [MemberData(nameof(GetTestResources))]
    public async Task AssertBehavior(string resourceFileName)
    {
        await ValidateRequest(resourceFileName, resourceFileName);
    }

    [Theory]
    [MemberData(nameof(GetErroneousTestResources))]
    public async Task AssertBadRequest(string resourceFileName)
    {
        await ValidateBadRequest(resourceFileName, resourceFileName);
    }

    public static IEnumerable<object[]> GetTestResources()
    {
        var directory = new DirectoryInfo("../../../Interpreter/Api/Requests");
        return directory.GetFiles("*.json", SearchOption.AllDirectories)
            .Select(f => f.FullName)
            .Where(name => !name.Contains("expected_error"))
            .Select(file => file[(file.IndexOf("Requests", StringComparison.Ordinal) + 9)..])
            .Select(file => new object[] { file })
            .ToArray();
    }

    public static IEnumerable<object[]> GetErroneousTestResources()
    {
        var directory = new DirectoryInfo("../../../Interpreter/Api/Requests");
        return directory.GetFiles("*.json", SearchOption.AllDirectories)
            .Select(f => f.FullName)
            .Where(name => name.Contains("expected_error"))
            .Select(file => file[(file.IndexOf("Requests", StringComparison.Ordinal) + 9)..])
            .Select(file => new object[] { file })
            .ToArray();
    }

    private async Task ValidateRequest(string requestJsonFileName, string responseTextFileName)
    {
        var request = ReadFile("Interpreter/Api/Requests/" + requestJsonFileName);
        var body = new StringContent(request, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync("/interpreter/", body);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(RemoveWhiteSpaces(ReadFile("Interpreter/Api/Responses/" + responseTextFileName)), content.Replace("\\r", ""));
    }

    private async Task ValidateBadRequest(string requestJsonFileName, string responseTextFileName)
    {
        var request = ReadFile("Interpreter/Api/Requests/" + requestJsonFileName);
        var body = new StringContent(request, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync("/interpreter/", body);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
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