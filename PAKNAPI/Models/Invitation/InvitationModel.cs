using Microsoft.AspNetCore.Http;
using PAKNAPI.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.Invitation
{
    public class INVInvitationInsertModel
    {
        public INVInvitationInsertIN Model { get; set; }

        public List<INVFileAttachInsertIN> INVFileAttach { get; set; }

        public IFormFileCollection Files { get; set; }

        public List<INVInvitationUserMapInsertIN> InvitationUserMap { get; set; }
    }

    public class INVInvitationUpdateModel
    {
        public INVInvitationGetById Model { get; set; }

        public List<INVFileAttachGetAllByInvitationId> INVFileAttach { get; set; }

        public List<InvitationFileAttach> LtsDeleteFile { get; set; }

        public IFormFileCollection Files { get; set; }

        public List<INVInvitationUserMapGetByInvitationId> InvitationUserMap { get; set; }
    }

    public class InvitationFileAttach{

        public int Id { get; set; }
        public int InvitationId { get; set; }
        public string FileAttach { get; set; }
        public string Name { get; set; }
        public byte FileType { get; set; }
    }

}
