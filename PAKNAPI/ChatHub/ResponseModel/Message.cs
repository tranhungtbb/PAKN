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
        public string ToId { get; set; }

        public string SubTags { get; set; }
    }
}
