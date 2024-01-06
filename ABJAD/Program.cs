using ABJAD.Interpreter.Core;
using ABJAD.Lexer.Core;
using ABJAD.Lexer.Domain;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<InterpreterService, InterpreterWithTimeout>();
builder.Services.AddSingleton<StringUtils>();
builder.Services.AddScoped<ILexer, LexerService>();
builder.Services.AddScoped<Analyzer, LexicalAnalyzer>();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(config =>
        config.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));

builder.Services.AddMvc()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace ABJAD
{
    public partial class Program { }
}