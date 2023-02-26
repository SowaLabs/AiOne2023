using AiOne.Chatbot.Mouth;
using AiOneChatbot.Application.Chatbot.Dto;
using Newtonsoft.Json;

namespace AiOneChatbot.Application.Chatbot.LipSyncGeneration
{
	/// <summary>
	/// Singleton.
	/// </summary>
	public class LipSyncGenerator
	{
        private readonly Mouth _lips;

        public LipSyncGenerator(Mouth lips)
		{
			_lips = lips;
		}

        public LipSync GenerateLipSync(byte[] audioFile)
		{
			var lipSyncAnim = _lips.GetLipSyncAnimation(audioFile);

			var cues = JsonConvert.DeserializeObject<List<MouthCue>>(JsonConvert.SerializeObject(lipSyncAnim.Cues));

			double duration = 0;

			for (int i = 0; i < cues.Count - 1; i++)
			{
				cues[i].End = cues[i + 1].Start;
			}

			if (cues.Count > 0)
			{
				duration = (cues.Last().End = cues.Last().Start);
			}

			return new LipSync
			{
				Duration = duration,
				MouthCues = cues
			};
		}
	}
}
