using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Chat.ResponseModel
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AdminId { get; set; }
        public int roomType { get; set; }
    }
}
