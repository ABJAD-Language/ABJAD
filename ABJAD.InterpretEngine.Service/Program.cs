using ABJAD.InterpretEngine.Service.Core;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<InterpreterService, InterpreterWithTimeout>();

builder.Services.AddCors(options => 
    options.AddDefaultPolicy(config => 
        config.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace ABJAD.InterpretEngine.Service
{
    public partial class Program { }
}