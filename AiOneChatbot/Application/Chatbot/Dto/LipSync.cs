namespace AiOneChatbot.Application.Chatbot.Dto
{
	public class LipSync
	{
		/// <summary>
		/// In seconds.
		/// </summary>
		public double Duration { get; init; }

		public List<MouthCue> MouthCues { get; init; }
	}
}
