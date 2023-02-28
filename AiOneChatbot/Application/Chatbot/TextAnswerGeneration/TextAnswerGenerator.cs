using AiOne.Chatbot.Brain;

namespace AiOneChatbot.Application.Chatbot.TextAnswerGeneration;

/// <summary>
/// Singleton.
/// </summary>
public class TextAnswerGenerator
{
	private readonly Brain _brain;

	public TextAnswerGenerator(Brain brain)
	{
		_brain = brain;
	}

	public (string answer, string prompt, string lang) GetTextAnswer(string question, string sessionId)
	{
		var response = _brain.GetResponse(question, sessionId);

		return (response.Text, response.Prompt, response.Language);
	}

	public void DeleteHistory(string sessionId)
	{
		_brain.ClearHistory(sessionId);
	}

	public void DeleteHistoryAll()
	{
		_brain.ClearHistoryAll();
	}
}
