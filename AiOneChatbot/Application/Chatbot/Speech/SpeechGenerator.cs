namespace AiOneChatbot.Application.Chatbot.Speech;
/// <summary>
/// Scoped.
/// </summary>
public class SpeechGenerator
{
	public byte[] GenerateSpeechAudioFile(string textToSpeak)
	{
#pragma warning disable CA1416 // Validate platform compatibility
		using var synth = new System.Speech.Synthesis.SpeechSynthesizer();

		// configure
		synth.Rate = 0; // 0 seems to be the best speed to speak

		using var memoryStream = new MemoryStream();

		synth.SetOutputToWaveStream(memoryStream);
		synth.Speak(textToSpeak);

		// set the synthesizer output to null to release the stream
		synth.SetOutputToNull();

		return memoryStream.ToArray();
#pragma warning restore CA1416 // Validate platform compatibility
	}
}
