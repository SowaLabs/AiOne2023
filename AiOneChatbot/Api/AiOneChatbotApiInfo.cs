using Microsoft.OpenApi.Models;

namespace AiOneChatbot.Api;
public static class AiOneChatbotApiInfo
{
	public const string SERVICE_NAME = "AiOne Chatbot";
	public const string SERVICE_API_GROUP_CHATBOT = "chatbot";

	public static Version SERVICE_VERSION { get; }

	static AiOneChatbotApiInfo()
	{
		SERVICE_VERSION = typeof(AiOneChatbotApiInfo).Assembly.GetName().Version;
	}

	public static OpenApiInfo GetOpenApiInfo(string title)
	{
		return new OpenApiInfo
		{
			Title = title,
			Version = $"v{SERVICE_VERSION.ToString(3)}",
			Contact = new OpenApiContact
			{
				Name = "AiOne team (Miha Grcar, Gregor Mohorko, Rok Bajec, Mitja Belak)",
			},
			Description = "AiOne Chatbot API service."
		};
	}
}
