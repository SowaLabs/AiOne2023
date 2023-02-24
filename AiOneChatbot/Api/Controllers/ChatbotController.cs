using System.ComponentModel.DataAnnotations;
using AiOneChatbot.Application.Chatbot.Dto;
using AiOneChatbot.Application.Chatbot.Speech;
using AiOneChatbot.Application.Chatbot.TextAnswerGeneration;
using Microsoft.AspNetCore.Mvc;

namespace AiOneChatbot.Api.Controllers;
[ApiController]
[ApiExplorerSettings(GroupName = AiOneChatbotApiInfo.SERVICE_API_GROUP_CHATBOT)]
[Route("[controller]")]
public class ChatbotController : ControllerBase
{
    private readonly SpeechGenerator _speechGenerator;
    private readonly TextAnswerGenerator _textAnswerGenerator;

    public ChatbotController(
        SpeechGenerator speechGenerator,
        TextAnswerGenerator textAnswerGenerator
        )
    {
        _speechGenerator = speechGenerator;
        _textAnswerGenerator = textAnswerGenerator;
    }

    [HttpGet]
    public async Task<ChatbotResponse> GetBotResponse(
        [Required][FromQuery] string question
        )
    {
        try {
			string textAnswer = _textAnswerGenerator.GetTextAnswer(question);
			byte[] audioFile = await _speechGenerator.GenerateSpeechAudioFile(textAnswer);

			return new ChatbotResponse
			{
				AnswerText = textAnswer,
				AnswerSpeechAudioFile = Convert.ToBase64String(audioFile)
			};
		} catch(Exception e) {
            return new ChatbotResponse
            {
                AnswerText = e.ToString()
            };
        }
    }
}
