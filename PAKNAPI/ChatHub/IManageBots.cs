using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace SignalR.Hubs
{
    public interface IManageBots
    {
        ResultBot Response(string botname, string UserInput);
        void RemoveBot(string botname);
        Task<string> ReloadBots();
    }
}
