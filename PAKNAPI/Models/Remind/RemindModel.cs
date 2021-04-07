using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.Remind
{
    public class RMRemind
    {
        public int id { get; set; }
        public int PetitionId { get; set; }
        public string Content { get; set; }
    }


    public class RMFileAttach {
        public int id { get; set; }
        public int RemindId { get; set; }
        public string FileAttach { get; set; }
        public string Name { get; set; }
        public int FileType { get; set; }
    }

    public class RMForward {
        public int id {get; set;}
        public long SenderId { get; set;}
        public int SendOrgId { get; set;}
        public int ReceiveOrgId { get; set;}
        public DateTime DateSend { get; set;}
        public int IsView { get; set;}
    }

    public class RMRemindView {
        public RMRemind Model { get; set; }
        public List<RMFileAttach> Files { get; set; }
    }

    public class RMRemindInsertRequest
    {
        public RMRemind Model { get; set; }
        //public List<RMFileAttach> ltsFiles { get; set; }

        public RMForward Forward { get; set; }

        public IFormFileCollection Files { get; set; }
    }
}
