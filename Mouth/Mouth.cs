using AiOne.Chatbot.Mouth.Models;
using CliWrap;
using CliWrap.Buffered;

namespace AiOne.Chatbot.Mouth
{
    public class Mouth
    {
        private static string lsDataFolder = "/ls/data";
        private static string lsFolder = "/ls";

        public LipSyncAnimation GetLipSyncAnimation(byte[] speech)
        {
            var id = Guid.NewGuid().ToString("N");
            File.WriteAllBytes($"{lsDataFolder}/{id}.wav", speech);
            // call rhubarb
            var result = Cli.Wrap($"{lsFolder}/rhubarb")
                  .WithArguments($"-o {lsDataFolder}/{id}.out {lsDataFolder}/{id}.wav -r phonetic")
                  .ExecuteBufferedAsync()
                  .GetAwaiter()
                  .GetResult();
            // create lipsync animation object
            var anim = new LipSyncAnimation {
                Cues = new List<Cue>()
            };
            foreach (var line in File.ReadAllLines($"{lsDataFolder}/{id}.out"))
            {
                var fields = line.Split(new[] { ' ', '\t' }, 2);
                anim.Cues.Add(new Cue { 
                    Start = Convert.ToDouble(fields[0].Trim()),
                    Value = fields[1].Trim()
                });
            }
            return anim;
        }
    }
}