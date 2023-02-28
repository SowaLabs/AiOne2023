using System.ComponentModel.DataAnnotations;
using AiOneChatbot.Application.Chatbot.Dto;
using AiOneChatbot.Application.Chatbot.LipSyncGeneration;
using AiOneChatbot.Application.Chatbot.Speech;
using AiOneChatbot.Application.Chatbot.TextAnswerGeneration;
using Microsoft.AspNetCore.Mvc;

namespace AiOneChatbot.Api.Controllers;
[ApiController]
[ApiExplorerSettings(GroupName = AiOneChatbotApiInfo.SERVICE_API_GROUP_CHATBOT)]
[Route("[controller]")]
public class ChatbotController : ControllerBase
{
    private readonly LipSyncGenerator _lipSyncGenerator;
	private readonly SpeechGenerator _speechGenerator;
    private readonly TextAnswerGenerator _textAnswerGenerator;

    public ChatbotController(
        LipSyncGenerator lipSyncGenerator,
        SpeechGenerator speechGenerator,
        TextAnswerGenerator textAnswerGenerator
        )
    {
        _lipSyncGenerator = lipSyncGenerator;
        _speechGenerator = speechGenerator;
        _textAnswerGenerator = textAnswerGenerator;
    }

    [HttpGet]
    public async Task<ChatbotResponse> GetBotResponse(
        [Required][FromQuery] string question,
        [FromQuery] string sessionId
        )
    {
        try {
            var textAnswer = _textAnswerGenerator.GetTextAnswer(question, sessionId ?? HttpContext.Session.Id);
			byte[] audioFile = await _speechGenerator.GenerateSpeechAudioFile(textAnswer.answer, textAnswer.lang);
            LipSync lipSync = _lipSyncGenerator.GenerateLipSync(audioFile);

			return new ChatbotResponse
			{
				AnswerText = textAnswer.answer,
                Prompt = textAnswer.prompt,
                Language = textAnswer.lang,
				AnswerSpeechAudioFile = Convert.ToBase64String(audioFile),
                LipSyncAnimation = lipSync
			};
		} catch(Exception e) {
            return new ChatbotResponse
            {
                AnswerText = e.ToString()
            };
        }
    }

    [HttpDelete("history")]
    public void DeleteHistory(string sessionId)
    {
        _textAnswerGenerator.DeleteHistory(sessionId ?? HttpContext.Session.Id);
    }

    [HttpDelete("history/all")]
    public void DeleteHistoryAll()
    {
        _textAnswerGenerator.DeleteHistoryAll();
    }

    [HttpGet("session")]
    public string GetDefaultSessionId()
    {
        return HttpContext.Session.Id;
    }
}
