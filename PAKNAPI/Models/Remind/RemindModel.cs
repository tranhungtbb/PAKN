using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.Remind
{
    public class RMRemindModel
    {
        public int id { get; set; }
        public int RecommendationId { get; set; }
        public string Content { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public int UnitId { get; set; }
    }


    public class RMFileAttachModel {
        public int id { get; set; }
        public int RemindId { get; set; }
        public string FileAttach { get; set; }
        public string Name { get; set; }
        public int FileType { get; set; }
    }

    public class RMForwardModel {
        public int id {get; set;}
        public long SenderId { get; set;}
        public int SendOrgId { get; set;}
        public int ReceiveOrgId { get; set;}
        public DateTime DateSend { get; set;}
        public int IsView { get; set;}
    }

    public class RMRemindView {
        public RMRemindModel Model { get; set; }
        public List<RMFileAttach> Files { get; set; }
    }

    public class RMRemindInsertRequest
    {
        public RMRemindModel Model { get; set; }
        //public List<RMFileAttach> ltsFiles { get; set; }

        public RMForwardModel Forward { get; set; }

        public IFormFileCollection Files { get; set; }
    }
}
