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
        public List<DropdownObject> lstUnit { get; set; }
        public List<DropdownObject> lstField { get; set; }
        public List<DropdownObject> lstIndividual { get; set; }
        public List<DropdownObject> lstBusiness { get; set; }
        public List<DropdownObject> lstHashTag { get; set; }
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
}