using System.Text;
using OpenAI;
using AI.Dev.OpenAI.GPT;
using Newtonsoft.Json;
using AiOne.Chatbot.Brain.Models;

namespace AiOne.Chatbot.Brain
{
    public class Brain
    {
        private const string humanName = "Human";
        private const string aiName = "Johnny";

        private const string endToken = "###";

        private const int responseTokenLimit = 256;
        private const int tokenCountLimit = 4000;

        private const double temp = 0.7;

        private const string simModel = "text-similarity-davinci-001";
        private const string model = "text-davinci-003";

        private const double simCutOff = 0;
        private const int countCutOff = 8;

        private List<ChatItem> knowledgeBase = new List<ChatItem>();
        private List<ChatItem> chatHistory = new List<ChatItem>();
        private List<string> context = new List<string>();

        private OpenAIClient aiApi;

        public Brain(string apiKey, string knowledgeBasePath, int responseTokenLimit = responseTokenLimit, int simCutOffCount = countCutOff)
        {
            // API
            aiApi = new OpenAIClient(new OpenAIAuthentication(apiKey));
            // load knowledge base
            foreach (var jsonLine in File.ReadAllLines(knowledgeBasePath))
            {
                knowledgeBase.Add(JsonConvert.DeserializeObject<ChatItem>(jsonLine));
            }
            // create context
            context = new List<string>(new[] {
                $"This is a conversation between a human and an AI agent named {aiName}. {aiName} answers questions about the crypto and stock trading mobile app BISON.",
                $"{aiName}: Hello. How can I help you today?"
            });
        }

        public Response GetResponse(string text)
        {
            var embedding = Embed(text);
            // build context
            var knowledgeBaseRanked = new List<(double, ChatItem)>();
            foreach (var doc in knowledgeBase)
            {
                double cosSim = Math.Max(
                    CosSim(embedding, doc.EmbeddingQ),
                    CosSim(embedding, doc.EmbeddingQA)
                );
                if (cosSim > simCutOff)
                {
                    knowledgeBaseRanked.Add((cosSim, doc));
                }
            }
            knowledgeBaseRanked = knowledgeBaseRanked.OrderByDescending(x => x.Item1).ToList();
            // generate prompt
            chatHistory.Add(new ChatItem { Question = text });
            var prompt = GeneratePrompt(
                knowledgeBaseRanked.Select(x => x.Item2).Take(countCutOff).ToList()
            );
            var task = aiApi.CompletionsEndpoint.CreateCompletionAsync(prompt, temperature: temp, model: model, maxTokens: responseTokenLimit, topP: 1, stopSequences: new[] { $"{humanName}:", $"{aiName}:", endToken });
            task.Wait();
            var responseText = task.Result.ToString().Trim();
            chatHistory.Last().Answer = responseText;
            return new Response
            {
                Prompt = prompt,
                Text = responseText
            };
        }

        public void ClearHistory()
        {
            chatHistory.Clear();
        }

        private class ChatItem
        {
            public string Question { get; set; }
            public string Answer { get; set; }

            public double[] EmbeddingQ { get; set; }
            public double[] EmbeddingA { get; set; }
            public double[] EmbeddingQA { get; set; }

            public override string ToString()
            {
                return $"{humanName}: {Question}\n{aiName}: {Answer}\n";
            }
        }

        private class EmbeddingHelperObject
        {
            public double[] Embedding { get; set; }
        }

        private static double CosSim(double[] a, double[] b)
        {
            if (a.Length != b.Length)
            {
                throw new Exception("Vector length mismatch.");
            }
            double sim = 0;
            for (int i = 0; i < a.Length; i++)
            {
                sim += a[i] * b[i];
            }
            return sim;
        }

        private static int CountTokens(string text)
        {
            return GPT3Tokenizer.Encode(text).Count;
        }

        private string GeneratePrompt(List<ChatItem> knowledgeBaseRanked)
        {
            var prompt = new StringBuilder();
            var lastInput = chatHistory.Last().ToString().TrimEnd('\n');
            foreach (var item in context) { prompt.Append($"{item}\n"); }
            int tokensLeft = tokenCountLimit - responseTokenLimit - CountTokens(prompt.ToString()) - CountTokens(lastInput);
            // add knowledge base
            foreach (var item in knowledgeBaseRanked)
            {
                var itemStr = item.ToString();
                if (tokensLeft - CountTokens(itemStr) <= 0) { break; }
                prompt.Append(itemStr);
                tokensLeft -= CountTokens(itemStr);
            }
            // add chat history
            int i = chatHistory.Count - 1 - 1; // ignore the last entry
            for (; i >= 0; i--)
            {
                tokensLeft -= CountTokens(chatHistory[i].ToString()) + 1;
                if (tokensLeft < 0) { break; }
            }
            for (int j = i + 1; j < chatHistory.Count - 1; j++)
            {
                prompt.Append(chatHistory[j].ToString());
            }
            return prompt.ToString() + lastInput;
        }

        private double[] Embed(string text)
        {
            var task = aiApi.EmbeddingsEndpoint.CreateEmbeddingAsync(text, simModel);
            task.Wait();
            var embedding = task.Result;
            var embeddings = JsonConvert.DeserializeObject<EmbeddingHelperObject[]>(JsonConvert.SerializeObject(embedding.Data));
            if (embeddings.Length != 1)
            {
                throw new Exception("Unexpected response from Embeddings endpoint.");
            }
            return embeddings[0].Embedding;
        }
    }
}