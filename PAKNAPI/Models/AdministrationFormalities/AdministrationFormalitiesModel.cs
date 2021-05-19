using Microsoft.AspNetCore.Http;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using System;
using System.Collections.Generic;

namespace PAKNAPI.Models.AdministrationFormalities
{

	public class AdministrationFormalitiesInsertRequest
    {
        public long? UserId { get; set; }
        public string UserFullName { get; set; }
        public DAMAdministrationInsertIN Data { get; set; }
        public List<DAMFileObj> LstXoaFile { get; set; }
        public List<DAMCompositionProfileObj> LstXoaFileForm { get; set; }
        public List<DAMCompositionProfileCreateObj> LstCompositionProfile { get; set; }
        public List<DAMChargesCreateIN> LstCharges { get; set; }
        public List<DAMImplementationProcessCreateIN> LstImplementationProcess { get; set; }
        public List<DeleteTableObject> LstDelete { get; set; }
        public IFormFileCollection Files { get; set; }
    }
	public class AdministrationFormalitiesUpdateRequest
    {
        public long? UserId { get; set; }
        public string UserFullName { get; set; }
        public DAMAdministrationUpdateIN Data { get; set; }
        public List<DAMFileObj> LstXoaFile { get; set; }
        public List<DAMCompositionProfileObj> LstXoaFileForm { get; set; }
        public List<DAMCompositionProfileUpdateObj> LstCompositionProfile { get; set; }
        public List<DAMChargesUpdateIN> LstCharges { get; set; }
        public List<DAMImplementationProcessUpdateIN> LstImplementationProcess { get; set; }
        public List<DeleteTableObject> LstDelete { get; set; }
        public IFormFileCollection Files { get; set; }
    }
    public class DeleteTableObject
    {
        public int? Id { get; set; }
        public int? Type { get; set; }
    }

    public class DAMCompositionProfileCreateObj: DAMCompositionProfileCreateIN
    {
        public int? Id { get; set; }
        public int? Index { get; set; }
        public string Name { get; set; }
        public int FileType { get; set; }
        public string FileAttach { get; set; }
        public List<DAMCompositionProfileObj> Files { get; set; }
    }
    public class DAMCompositionProfileUpdateObj : DAMCompositionProfileUpdateIN
    {
        public int? Index { get; set; }
    }
    public class DAMFileObj : DAMAdministrationFilesInsertIN
    {
        public int? Id { get; set; }
    }

    public class DAMCompositionProfileObj : DAMCompositionProfileFileFilesInsertIN
    {
        public int? Id { get; set; }
    }

    public class AdministrationGetById
    {
        public DAMAdministrationGetById Data { get; set; }
        public List<DAMFileObj> Files { get; set; }
        public List<DAMCompositionProfileCreateObj> LstCompositionProfile { get; set; }
        public List<DAMChargesGetById> LstCharges { get; set; }
        public List<DAMImplementationProcessUpdateIN> LstImplementationProcess { get; set; }

    }
}