using System.ComponentModel.DataAnnotations;
using AiOneChatbot.Application.Chatbot.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AiOneChatbot.Controllers;
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("[controller]")]
public class ChatbotController : ControllerBase
{
    public ChatbotController()
    {

    }

    [HttpGet]
    public ChatbotResponse GetBotResponse(
        [Required][FromQuery] string question
        )
    {
        // FIXME
        return new ChatbotResponse
        {
            AnswerText = $"You have asked me this question: '{question}'."
        };
    }
}
