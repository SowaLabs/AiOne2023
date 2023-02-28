using AiOne.Chatbot.Brain;
using AiOne.Chatbot.Mouth;
using AiOneChatbot.Api;
using AiOneChatbot.Application.Chatbot.LipSyncGeneration;
using AiOneChatbot.Application.Chatbot.Speech;
using AiOneChatbot.Application.Chatbot.TextAnswerGeneration;
using AiOneChatbot.Application.Config;
using GM.Utility;
using GM.WebAPI;
using Microsoft.AspNetCore.Session;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace AiOneChatbot;
public class Program
{
    public static Task Main(string[] args)
    {
        return GMWebApi.Run<Program, AiOneChatbotConfig>(
			args: args,
			serviceConfigurator: ConfigureServices,
			setupSwaggerGeneration: SetupSwaggerGeneration,
			setupSwaggerUI: SetupSwaggerUI,
			addCorsMiddleWare: false,
			addAuthMiddleware: false,
			xmlDocTypes: new Type[] { typeof(Program) },
			additionalMiddleware: new List<Type>(new[] { 
				typeof(SessionMiddleware), 
				typeof(RetainSessionMiddleware) 
			}));
	}

	private static void ConfigureServices(
		IServiceCollection services,
		AiOneChatbotConfig config
		)
	{
		// services
		services.AddScoped<SpeechGenerator>();
		services.AddSingleton(new Brain(
			"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",
            //$"{AppContext.BaseDirectory}/FAQ_embeddings.jsonl"
            "/models/FAQ_embeddings.jsonl"
        ));
		services.AddSingleton(new Mouth());
		services.AddSingleton<LipSyncGenerator>();
		services.AddSingleton<TextAnswerGenerator>();

        services.AddDistributedMemoryCache();
        services.AddSession(options =>
        {
			options.IdleTimeout = TimeSpan.FromMinutes(5); // WARNME: hardcoded
        });

        string[] allowedOrigins = new string[] { "*" };
		services.AddCors(options =>
		{
			options.AddDefaultPolicy(corsPolicyBuilder =>
			{
				corsPolicyBuilder
					.WithOrigins(allowedOrigins)
					.AllowAnyHeader()
					.AllowCredentials()
					.AllowAnyMethod();
			});
		});
	}

	private static void SetupSwaggerGeneration(SwaggerGenOptions swaggerGenOptions)
	{
		// documents
		swaggerGenOptions.SwaggerDoc(
			name: AiOneChatbotApiInfo.SERVICE_API_GROUP_CHATBOT,
			info: AiOneChatbotApiInfo.GetOpenApiInfo($"{AiOneChatbotApiInfo.SERVICE_NAME} {AiOneChatbotApiInfo.SERVICE_API_GROUP_CHATBOT.ToTitleCase()} API")
			);

		// security definitions
	}

	private static void SetupSwaggerUI(SwaggerUIOptions swaggerUIOptions)
	{
		// endpoints
		swaggerUIOptions.SwaggerEndpoint(
			url: $"{AiOneChatbotApiInfo.SERVICE_API_GROUP_CHATBOT}/swagger.json",
			name: $"{AiOneChatbotApiInfo.SERVICE_NAME} {AiOneChatbotApiInfo.SERVICE_API_GROUP_CHATBOT.ToTitleCase()} API"
			);
	}

    private class RetainSessionMiddleware
    {
        private readonly RequestDelegate _next;

        public RetainSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
		{
            httpContext.Session.Set("Init", new byte[] { 0 });
            return _next(httpContext);
        }
    }
}
