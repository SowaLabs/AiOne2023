using System.Reflection;
using AiOneChatbot.Application.Chatbot.Speech;
using AiOneChatbot.Application.Chatbot.TextAnswerGeneration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

Version VERSION = Version.Parse("1.0.0");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagOptions =>
{
    swagOptions.SwaggerDoc($"v{VERSION.Major}", new OpenApiInfo
    {
        Title = "AiOne Chatbot API",
        Version = VERSION.ToString(3),
        Contact = new OpenApiContact
        {
            Name = "AiOne team (Miha Grcar, Gregor Mohorko, Rok Bajec, Mitja Belak)",
        },
        Description = "\nAiOne Chatbot API service."
	});

    swagOptions.EnableAnnotations();
    swagOptions.UseAllOfToExtendReferenceSchemas();

	// Use method name as operationId
	swagOptions.CustomOperationIds(apiDesc => apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null);
}).AddSwaggerGenNewtonsoftSupport();

builder.Services.ConfigureSwaggerGen(options =>
{
    var xmlDocFile = Path.Combine(AppContext.BaseDirectory, $"{typeof(Program).Assembly.GetName().Name}.xml");
    if(File.Exists(xmlDocFile)) {
        options.IncludeXmlComments(xmlDocFile);
    }
});

// services
builder.Services.AddScoped<SpeechGenerator>();
builder.Services.AddSingleton<TextAnswerGenerator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint($"v{VERSION.Major}/swagger.json", "AiOne Chatbot API");
    });
}

app.UseRouting();

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
