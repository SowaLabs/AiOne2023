using System.Security.Policy;

namespace AiOneChatbot.Application.Chatbot.Dto
{
	public class ChatbotResponse
	{
		public string AnswerText { get; init; }

		public string Prompt { get; init; }

		public string Language { get; init; }

		/// <summary>
		/// Base64 encoded.
		/// </summary>
		public string AnswerSpeechAudioFile { get; init; }

		public LipSync LipSyncAnimation { get; init; }
	}
}
