using Google.Cloud.TextToSpeech.V1;

//using System.Speech.Synthesis;

namespace AiOneChatbot.Application.Chatbot.Speech;
/// <summary>
/// Scoped.
/// </summary>
public class SpeechGenerator
{
	public async Task<byte[]> GenerateSpeechAudioFile(string textToSpeak)
	{
		var clientBuilder = new TextToSpeechClientBuilder
		{
			JsonCredentials = CREDENTIALS
		};
		var client = await clientBuilder.BuildAsync();

		// TO OUTPUT AVAILABLE VOICES:
		//var listEnVoicesResponse = await client.ListVoicesAsync("en");
		//var listDeVoicesResponse = await client.ListVoicesAsync("de");
		//foreach(var voice in listEnVoicesResponse.Voices.Concat(listDeVoicesResponse.Voices)) {
		//	Console.WriteLine($"{voice.Name} ({voice.SsmlGender}); Language codes: {string.Join(", ", voice.LanguageCodes)}");
		//}

		var input = new SynthesisInput
		{
			Text = textToSpeak
		};
		var voiceSelection = new VoiceSelectionParams
		{
			// selected from: https://cloud.google.com/text-to-speech/docs/voices
			LanguageCode = "en-US",
			Name = "en-GB-Standard-D"
			//SsmlGender = SsmlVoiceGender.Male
		};
		var audioConfig = new AudioConfig
		{
			AudioEncoding = AudioEncoding.Linear16,
			//Pitch = 
			//SpeakingRate = 
		};

		var response = await client.SynthesizeSpeechAsync(input, voiceSelection, audioConfig);

		return response.AudioContent.ToByteArray();


		// using System.Speech:
#pragma warning disable CA1416 // Validate platform compatibility
		//using var synth = new SpeechSynthesizer();

		//// configure
		////synth.Rate = 0; // 0 seems to be the best speed to speak anyway

		//using var memoryStream = new MemoryStream();

		//synth.SetOutputToWaveStream(memoryStream);
		//synth.Speak(textToSpeak);

		//// set the synthesizer output to null to release the stream
		//synth.SetOutputToNull();

		//return memoryStream.ToArray();
#pragma warning restore CA1416 // Validate platform compatibility
	}

	private const string CREDENTIALS = "{\r\n  \"type\": \"service_account\",\r\n  \"project_id\": \"ferrous-layout-378812\",\r\n  \"private_key_id\": \"59f3b11022503193dacc0ca467fd388c2cbaf66f\",\r\n  \"private_key\": \"-----BEGIN PRIVATE KEY-----\\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCeKfNRefSBldvI\\nW2KNmYJMmS/3kHlOykV1fig75rrIJMozIO84LmXGaIet4u1Dseqi/art7ac312Na\\nFzGVPjRTz9bF5evN+IA3NgapD2paXUg1z5WEZHlNlJeggHjl763Wpd4kxXhXWi9o\\n8bc1zhs9gw9wj2R6EELNsUoPqnGk2lQlQ+J97dL3rlJovKtcmXLPH9kQm4QtbbMw\\nAR7ImskbNv3CHzxxr5pJug+tGH1REgMS1J7YfHI40hN0NMfXK5mHhXhhzn0sN2SX\\nQhlnAu8IVRTu9uNtUv0G/mpCH6qND8SxM/bH88KttVUiyz7wYU0XvtOeTbqBsFAC\\n2C+GOeshAgMBAAECggEAAcdU/iiR8FcgFpu4ewEr7vKWEIGma5VovI6Do1DP7upC\\n/9ox9n7HFDketXzk0CsTCWwy3OQcSkt4yY2TOCtv6TVgI++W/ET8JCLOZl5PAt8j\\nrb883jVHC/FM1zg6o9NTMCPDYMR7uve/qr11IvzDX6i2prxmE91U8v04aP6wsnrh\\nhnNVdOyBiwfkG8AZsdPqDFyQneZ1stY+aERTkDngBW7JP5S6vQYMn6KlOh23As2k\\n4lHzkmIOrL7QhDZ97/8Z5noC4jslJuQLpx3XQIvo82fZv2xrwZVUquLGqdaCRyMQ\\nMi+2wflzcsZMECWxBArY7uI+jxE1iw3w9r2AlmSVsQKBgQDbXmga5wKy7PsXk6ql\\nDqLeXN27clei01zGlIsP2AxZqtxqIN7b7LjrRUE0B4LmTJSDZ7ucvjJz8YBJyNzv\\nhfN0tLp/2ckiMW+iBqvDsDLJToXHMEVK2+DmzlfsvV8qHe4J7iM9sf3SWB2Z2Slk\\nSxtxqWDcutjnlEbCUaxqQOvmrQKBgQC4kybXIYaT3yFK72WVF5CANDda8DbR9xcX\\ngHXFG9LHNmmfqOamE3Pai9jGioLJ5gr6/dM6QgQnrqk65bIa1u/uFo7A3tl0q9zC\\ngiyccJdbG9UnycAfVw9Z4U92Asgab1xRFGCiRMfw82FA3VfPn2pWfOTucZrSJCBB\\nYoHM8uwIxQKBgCXbfv/ViiOyvgptk290vSq+wA1PSExzSXmDvRP45vi6gdtW3N77\\nVVZU11HRUgfIg4DB8CC2uiJENS2GmopDhaZIYj8aKJncCjXeDNpyl/EhufiMHgkz\\nNPbf+VCDxd6Q17mi/TXyd6lLR6B7V6dVRuEwp+Sv4irk7ekvCyAzIALpAoGAbycW\\n7dlfvS9TUlhG+Xk+aSaI63fb26gMvPd8dN5jmdqX0zGY7Qyr6UHsouwJBWNG243+\\nJYhaDjet7C0li+pChUQKZfYOSKezA2P3T6KyU4i4XgSucXExanQ4MR5NuTW1B5LY\\n+v+m1OEMgU6KshsTo9AE4d+CV+ivkBCtkGn+YO0CgYEAyP9Hwc5ZGrruOV4wxfkL\\nQMKG3+DERTYRshtGQEHQyC85gKsFqaVc+X5XNy5pnrMFd83X+L9Q0yNKm0jrW2cw\\nNDEu9ftBdK/p1jsRAdE5X2BoMl25+LrlAvCqPEJERc/WljD+ahVGXV4B5HgtTWla\\n+Bf6Mbq21b4d5E0VgkIyjcU=\\n-----END PRIVATE KEY-----\\n\",\r\n  \"client_email\": \"my-text-to-speech-sa@ferrous-layout-378812.iam.gserviceaccount.com\",\r\n  \"client_id\": \"115009584301627924879\",\r\n  \"auth_uri\": \"https://accounts.google.com/o/oauth2/auth\",\r\n  \"token_uri\": \"https://oauth2.googleapis.com/token\",\r\n  \"auth_provider_x509_cert_url\": \"https://www.googleapis.com/oauth2/v1/certs\",\r\n  \"client_x509_cert_url\": \"https://www.googleapis.com/robot/v1/metadata/x509/my-text-to-speech-sa%40ferrous-layout-378812.iam.gserviceaccount.com\"\r\n}\r\n";
}
