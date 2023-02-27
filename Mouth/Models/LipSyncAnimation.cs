namespace AiOne.Chatbot.Mouth.Models
{
    public class Cue
    {
        public double Start { get; init; }
        public string Value { get; init; }
    }

    public class LipSyncAnimation
    { 
        public List<Cue> Cues { get; set; } 
    }
}
