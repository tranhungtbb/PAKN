using Microsoft.AspNetCore.Http;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using System;
using System.Collections.Generic;

namespace PAKNAPI.Models.Recommendation
{

	public class RecommendationGetDataForCreateResponse
    {
        public string Code { get; set; }
        public List<DropdownTree> lstUnit { get; set; }
        public List<DropdownObject> lstField { get; set; }
        public List<DropdownObject> lstIndividual { get; set; }
        public List<DropdownObject> lstBusiness { get; set; }
        public List<DropdownObject> lstHashTag { get; set; }
    }

	public class RecommendationGetDataForForwardResponse
    {
        public List<DropdownObject> lstUnitNotMain { get; set; }
    }

	public class RecommendationGetDataForProcessResponse
    {
        public List<DropdownObject> lstHashtag { get; set; }
        public List<DropdownObject> lstUsers { get; set; }
        public List<DropdownObject> lstGroupWord { get; set; }
    }
	public class RecommendationInsertRequest
    {
        public long? UserId { get; set; }
        public int? UserType { get; set; }
        public string UserFullName { get; set; }
        public MRRecommendationInsertIN Data { get; set; }
        public List<DropdownObject> ListHashTag { get; set; }
        public List<MRRecommendationFiles> LstXoaFile { get; set; }
        public IFormFileCollection Files { get; set; }
    }
	public class RecommendationUpdateRequest
    {
        public long? UserId { get; set; }
        public int? UserType { get; set; }
        public string UserFullName { get; set; }
        public MRRecommendationUpdateIN Data { get; set; }
        public List<DropdownObject> ListHashTag { get; set; }
        public List<MRRecommendationFiles> LstXoaFile { get; set; }
        public IFormFileCollection Files { get; set; }
    }
    public class RecommendationGetByIDResponse
    {
        public MRRecommendationGetByID Model { get; set; }
        public List<MRRecommendationHashtagGetByRecommendationId> lstHashtag { get; set; }
        public List<MRRecommendationFilesGetByRecommendationId> lstFiles { get; set; }
    }
    public class RecommendationGetByIDViewResponse
    {
        public MRRecommendationGetByIDView Model { get; set; }
        public MRRecommendationConclusionGetByRecommendationId ModelConclusion { get; set; }
        public List<MRRecommendationHashtagGetByRecommendationId> lstHashtag { get; set; }
        public List<MRRecommendationFilesGetByRecommendationId> lstFiles { get; set; }
        public List<MRRecommendationConclusionFilesGetByConclusionId> filesConclusion { get; set; }
    }

    public class RecommendationForwardRequest
    {
        public MRRecommendationForwardInsertIN _mRRecommendationForwardInsertIN { get; set; }
        public List<DropdownObject> ListHashTag { get; set; }
        public byte RecommendationStatus { get; set; }
        public bool IsList { get; set; }
    }
    public class RecommendationForwardProcess
    {
        public MRRecommendationForwardProcessIN _mRRecommendationForwardProcessIN { get; set; }
        public List<DropdownObject> ListHashTag { get; set; }
        public byte RecommendationStatus { get; set; }
        public bool? ReactionaryWord { get; set; }
        public string ListGroupWordSelected { get; set; }
        public bool IsList { get; set; }
        public bool? IsForwardProcess { get; set; }
    }
    public class RecommendationOnProcessConclusionProcess
    {
        public MRRecommendationConclusionInsertIN DataConclusion { get; set; }
        public IFormFileCollection Files { get; set; }
        public byte RecommendationStatus { get; set; }        
        public List<DropdownObject> ListHashTag { get; set; }
    }

    public class RecommendationSendProcess
    {
        public int? id { get; set; }
        public byte? status { get; set; }
    }
}