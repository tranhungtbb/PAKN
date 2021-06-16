using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.ModelBase
{
    public class EmailManagementModelBase
    {
        public long? Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Signature { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UserCreatedId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UserUpdateId { get; set; }
        public DateTime? SendDate { get; set; }
        public int? UserSend { get; set; }
        public int? Unit { get; set; }
        public int? Status { get; set; }
    }

    public class EmailManagementPagedListModel : EmailManagementModelBase
    {
        public long? RowNumber { get; set; }
        public int? ObjectId { get; set; }
        public string ObjectName { get; set; }
        public string UnitName { get; set; }
    }
    public class EmailManagementAttachmentModel
    {
        public int? Id { get; set; }
        public long? EmailId { get; set; }
        public string FileAttach { get; set; }
        public string Name { get; set; }
        public int FileType { get; set; }
    }
    public class EmailManagementIndividualModel
    {
        public int? Id { get; set; }
        public long? EmailId { get; set; }
        public int? IndividualId { get; set; }
        public string IndividualFullName { get; set; }
        public string UnitName { get; set; }
    }
    public class EmailManagementBusinessModel
    {
        public int? Id { get; set; }
        public long? EmailId { get; set; }
        public int? BusinessId { get; set; }
        public string RepreFullName { get; set; }
        public string BusinessName { get; set; }
        public string UnitName { get; set; }
    }

    public class EmailMangementADO
    {
        private SQLCon _sQLCon;

        public EmailMangementADO(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }

        public EmailMangementADO()
        {
        }

        public async Task<EmailManagementModelBase> Insert(EmailManagementModelBase model)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("Title", model.Title);
            DP.Add("Content", model.Content);
            DP.Add("Signature", model.Signature);
            DP.Add("CreatedDate", DateTime.Now);
            DP.Add("UserCreatedId", model.UserCreatedId);
            DP.Add("UpdateDate", DateTime.Now);
            DP.Add("UserUpdateId", model.UserUpdateId);
            DP.Add("SendDate", model.SendDate);
            DP.Add("UserSend", model.UserSend);
            DP.Add("Unit", model.Unit);
            //DP.Add("IdOut", model.Id, null, System.Data.ParameterDirection.Output);
            var rs = await _sQLCon.ExecuteNonQueryDapperAsync("Email_QuanLyTinNhanInsert", DP);
            model.Id = rs;
            return model;
        }
        public async Task<EmailManagementModelBase> Update(EmailManagementModelBase model)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("Title", model.Title);
            DP.Add("Content", model.Content);
            DP.Add("Signature", model.Signature);
            //DP.Add("CreatedDate", model.CreatedDate);
            //DP.Add("UserCreatedId", model.UserCreatedId);
            DP.Add("UpdateDate", DateTime.Now);
            DP.Add("UserUpdateId", model.UserUpdateId);
            DP.Add("SendDate", model.SendDate);
            DP.Add("UserSend", model.UserSend);
            DP.Add("Unit", model.Unit);
            DP.Add("Id", model.Id);
            var rs = await _sQLCon.ExecuteNonQueryDapperAsync("Email_QuanLyTinNhanUpdate", DP);
            return model;
        }

        public async Task<IEnumerable<EmailManagementModelBase>> GetById(long id)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("Id", id);
            return await _sQLCon.ExecuteListDapperAsync<EmailManagementModelBase>("[Email_QuanLyTinNhanGetById]", DP);
        }

        public async Task<IEnumerable<EmailManagementPagedListModel>> GetPagedList(
            string title,
            int? unit,
            int? objectId,
            short? status,
            int pageIndex = 1,
            int pageSize = 20)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("Title", title);
            DP.Add("Unit", unit);
            DP.Add("ObjectId", objectId);
            DP.Add("Status", status);
            DP.Add("PageIndex", pageIndex);
            DP.Add("PageSize", pageSize);
            var rs = await _sQLCon.ExecuteListDapperAsync<EmailManagementPagedListModel>("[Email_QuanLyTinNhan_GetPagedList]", DP);
            return rs;
        }
    }

    public class EmailManagementAttachmentADO
    {
        private SQLCon _sQLCon;

        public EmailManagementAttachmentADO(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }

        public EmailManagementAttachmentADO()
        {
        }

        public async Task<int> Insert(EmailManagementAttachmentModel model)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("EmailId", model.EmailId);
            DP.Add("FileAttach", model.FileAttach);
            DP.Add("Name", model.Name);
            DP.Add("FileType", model.FileType);
            return await _sQLCon.ExecuteNonQueryDapperAsync("Email_quanlytinnhan_AttachmentInsert", DP);
        }
        public async Task<int> Delete(string Ids)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("Ids", Ids);
            return await _sQLCon.ExecuteNonQueryDapperAsync("Email_quanlytinnhan_AttachmentDeleteMulti", DP);
        }
        public async Task<IEnumerable<EmailManagementAttachmentModel>> GetByEmailId(long emailId)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("EmailId", emailId);
            return await _sQLCon.ExecuteListDapperAsync<EmailManagementAttachmentModel>("[Email_AttachmentByEmailId]", DP);
        }
    }

    public class EmailManagementIndividualADO
    {
        private SQLCon _sQLCon;

        public EmailManagementIndividualADO(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }

        public EmailManagementIndividualADO()
        {
        }

        public async Task<int> Update(EmailManagementIndividualModel model)
        {
            DynamicParameters DP = new DynamicParameters();
            if (model.Id.HasValue && model.Id > 0 )
            {
                DP.Add("EmailId", model.EmailId);
                DP.Add("IndividualId", model.IndividualId);
                DP.Add("Id", model.Id);
                return await _sQLCon.ExecuteNonQueryDapperAsync("[Email_quanlytinnhan_IndividualUpdate]", DP);
            }
            else
            {
                DP = new DynamicParameters();
                DP.Add("EmailId", model.EmailId);
                DP.Add("IndividualId", model.IndividualId);
                DP.Add("UnitName", model.UnitName);
                return await _sQLCon.ExecuteNonQueryDapperAsync("[Email_quanlytinnhan_IndividualInsert]", DP);
            }
            
            
        }
        public async Task<int> Delete(string Ids)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("Ids", Ids);
            return await _sQLCon.ExecuteNonQueryDapperAsync("[Email_quanlytinnhan_IndividualDelMuti]", DP);
        }
        public async Task<IEnumerable<EmailManagementIndividualModel>> GetByEmailId(long emailId)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("EmailId", emailId);
            return await _sQLCon.ExecuteListDapperAsync<EmailManagementIndividualModel>("Email_Individual_GetByEmailId", DP);
        }
    }
    public class EmailManagementBusinessADO
    {
        private SQLCon _sQLCon;

        public EmailManagementBusinessADO(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }

        public EmailManagementBusinessADO()
        {
        }

        public async Task<int> Update(EmailManagementBusinessModel model)
        {
            DynamicParameters DP = new DynamicParameters();
            if (model.Id.HasValue && model.Id > 0)
            {
                DP.Add("EmailId", model.EmailId);
                DP.Add("BusinessId", model.BusinessId);
                DP.Add("Id", model.Id);
                return await _sQLCon.ExecuteNonQueryDapperAsync("[Email_quanlytinnhan_BusinessUpdate]", DP);
            }
            else
            {
                DP = new DynamicParameters();
                DP.Add("EmailId", model.EmailId);
                DP.Add("BusinessId", model.BusinessId);
                DP.Add("UnitName", model.UnitName);
                return await _sQLCon.ExecuteNonQueryDapperAsync("[Email_quanlytinnhan_BusinessInsert]", DP);
            }


        }
        public async Task<int> Delete(string Ids)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("Ids", Ids);
            return await _sQLCon.ExecuteNonQueryDapperAsync("Email_quanlytinnhan_BusinessDelMuti", DP);
        }
        public async Task<IEnumerable<EmailManagementBusinessModel>> GetByEmailId(long emailId)
        {
            DynamicParameters DP = new DynamicParameters();
            DP.Add("EmailId", emailId);
            return await _sQLCon.ExecuteListDapperAsync<EmailManagementBusinessModel>("Email_Business_GetByEmailId", DP);
        }
    }
}
