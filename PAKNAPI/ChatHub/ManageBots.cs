using KarmaloopAIMLBot;
using PAKNAPI.Common;
using PAKNAPI.Models.ModelBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SignalR.Hubs
{
    public class ResultBot
    {
        public string Answer { get; set; }
        public List<string> SubTags { get; set; }
    }

    public class ResultBotNew
    {
        public string Answer { get; set; }
        public string SubTags { get; set; }
    }


    public class ManageBots : IManageBots
    {
        AIMLBot AI;
        private readonly static Dictionary<string, BotUser> _ConnectionsMap = new Dictionary<string, BotUser>();
        private readonly IAppSetting _appSetting;
        public ManageBots(IAppSetting appSetting)
        {
            _appSetting = appSetting;
            AI = new AIMLBot();
            AI.LoadSettings();
            AI.LoadAIMLFromFiles();
            Task.Run(async()=> {
                await this.ReloadBots();
            });
        }

        public async Task<string> ReloadBots()
        {
            string result = await new BotGetLibrary(_appSetting).BotGetAllLibrary();
            string path = Path.Combine(Environment.CurrentDirectory, "customaiml.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            AI.loadAIMLFromXML(doc, "customaiml.xml");
            return result;
        }

        public ResultBot Response(string botname, string UserInput)
        {
            BotUser bot;
            if (!_ConnectionsMap.ContainsKey(botname))
            {
                bot = new BotUser(Guid.NewGuid(), AI);
                _ConnectionsMap.Add(botname, bot);
            }
            else
            {
                bot = _ConnectionsMap[botname];
            }
            Request r = new Request(UserInput, bot, AI); //With This Code it will Request The Response From AIML Folders
            Result res = AI.Chat(r);
            ResultBot resl = new ResultBot()
            {
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
