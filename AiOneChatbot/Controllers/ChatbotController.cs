using System.ComponentModel.DataAnnotations;
using AiOneChatbot.Application.Chatbot.Dto;
using AiOneChatbot.Application.Chatbot.Speech;
using AiOneChatbot.Application.Chatbot.TextAnswerGeneration;
using Microsoft.AspNetCore.Mvc;

namespace AiOneChatbot.Controllers;
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
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
    public ChatbotResponse GetBotResponse(
        [Required][FromQuery] string question
        )
    {
        string textAnswer = _textAnswerGenerator.GetTextAnswer(question);
        byte[] audioFile = _speechGenerator.GenerateSpeechAudioFile(textAnswer);

		return new ChatbotResponse
        {
            AnswerText = textAnswer,
            AnswerSpeechAudioFile = Convert.ToBase64String(audioFile)
		};
    }
}
