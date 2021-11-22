using KarmaloopAIMLBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaloopAIMLBotParser
{
    public class ResultBot
    {
        public string Answer { get; set; }
        public List<string> SubTags { get; set; }
    }

        public class ManageBots
    {
        AIMLBot AI;
        private readonly static Dictionary<string, BotUser> _ConnectionsMap = new Dictionary<string, BotUser>();
        public ManageBots() {
            AI = new AIMLBot();
            AI.LoadSettings();
            AI.LoadAIMLFromFiles();
        }

        public ResultBot Response(string botname,string UserInput)
        {
            BotUser bot; 
            if (!_ConnectionsMap.ContainsKey(botname)) {
                bot = new BotUser(Guid.NewGuid(), AI);
                _ConnectionsMap.Add(botname, bot);
            }
            else {
                bot = _ConnectionsMap[botname];
            }
            Request r = new Request(UserInput, bot, AI); //With This Code it will Request The Response From AIML Folders
            Result res = AI.Chat(r);
            ResultBot resl = new ResultBot() {
                Answer = res.Output,
                SubTags = res.subTags
            };
            return resl;
        }

        public void RemoveBot(string botname)
        {
      
            if (!_ConnectionsMap.ContainsKey(botname))
            {
                _ConnectionsMap.Remove(botname);
            }
            
      
        }
    }
}
