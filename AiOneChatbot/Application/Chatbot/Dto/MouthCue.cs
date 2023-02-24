namespace AiOneChatbot.Application.Chatbot.Dto
{
	public class MouthCue
	{
		/// <summary>
		/// In seconds.
		/// </summary>
		public double Start { get; init; }

		/// <summary>
		/// In seconds.
		/// </summary>
		public double End { get; init; }

		/// <summary>
		/// The mouth shape to display.
		/// </summary>
		public string Value { get; init; }
	}
}
