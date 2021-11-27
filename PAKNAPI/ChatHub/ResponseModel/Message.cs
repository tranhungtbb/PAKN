using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Chat.ResponseModel
{
    public class Message
    {

        public string Content { get; set; }
        public string Timestamp { get; set; }
        public string From { get; set; }

        public string FromId { get; set; }
        public string To { get; set; }


        public List<string> SubTags { get; set; }
    }
}
