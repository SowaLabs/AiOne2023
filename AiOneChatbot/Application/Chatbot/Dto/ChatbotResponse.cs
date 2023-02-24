namespace AiOneChatbot.Application.Chatbot.Dto
{
	public class ChatbotResponse
	{
		public string AnswerText { get; init; }

		/// <summary>
		/// Base64 encoded.
		/// </summary>
		public string AnswerSpeechAudioFile { get; init; }
	}
}
