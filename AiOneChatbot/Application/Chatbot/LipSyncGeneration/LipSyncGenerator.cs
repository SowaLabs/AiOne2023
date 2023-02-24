using AiOneChatbot.Application.Chatbot.Dto;

namespace AiOneChatbot.Application.Chatbot.LipSyncGeneration
{
	/// <summary>
	/// Singleton.
	/// </summary>
	public class LipSyncGenerator
	{
		public LipSync GenerateLipSync(byte[] audioFile)
		{
			// FIXME
			return new LipSync
			{
				Duration = 0.47,
				MouthCues = new List<MouthCue>
				{
					new MouthCue { Start = 0, End = 0.05, Value = "X" },
					new MouthCue { Start = 0.05, End = 0.27, Value = "D" },
					new MouthCue { Start = 0.27, End = 0.31, Value = "C" },
					new MouthCue { Start = 0.31, End = 0.43, Value = "B" },
					new MouthCue { Start = 0.43, End = 0.47, Value = "X" }
				}
			};
		}
	}
}
