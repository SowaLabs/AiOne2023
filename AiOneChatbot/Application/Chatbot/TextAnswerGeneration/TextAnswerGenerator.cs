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

	public (string answer, string prompt, string lang) GetTextAnswer(string question)
	{
		var response = _brain.GetResponse(question);

		return (response.Text, response.Prompt, response.Language);
	}

	public void DeleteHistory()
	{
		_brain.ClearHistory();
	}
}
