using System;
using System.Collections.Generic;
using System.IO;
using LexEngine.Tokens;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace LexEngine.Test;

public class LexerIntegrationTest
{
    // [Fact]
    // private void LexTest1()
    // {
    //     var input = File.ReadAllText("../../..//Resources/integration_test_1.txt");
    //     var output = File.ReadAllText("../../../Resources/integration_test_1.json");
    //     var expectedTokens = JsonConvert.DeserializeObject<List<Token>>(output);
    //     var lexer = new Lexer(input, new StringUtils());
    //     var actualTokens = lexer.Lex();
    //
    //     Assert.Equal(expectedTokens, actualTokens);
    // }
}