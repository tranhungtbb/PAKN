using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Chat.ResponseModel
{
    public enum BotStatus
    {
        Enable = 1,
        Disable = 2,
       
    }

    public class MessageTypes
    {
        public static string Conversation { get { return "Conversation"; } }
        public static string All { get { return "All"; } }
       
    }
    public class Message
    {
        public string HiddenAnswer { get; set; }
        public string Content { get; set; }
        public string Timestamp { get; set; }
        public string From { get; set; }

        public string FromId { get; set; }
        public string To { get; set; }

        public string FromFullName { get; set; }

        public string FromAvatarPath { get; set; }
        public List<string> SubTags { get; set; }

        public string Type { get; set; }
    }
}
