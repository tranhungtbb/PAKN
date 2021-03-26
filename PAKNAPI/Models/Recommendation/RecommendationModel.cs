using Microsoft.AspNetCore.Http;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using System;
using System.Collections.Generic;

namespace PAKNAPI.Models.Recommendation
{

	public class RecommendationGetDataForCreateResponse
    {
        public List<DropdownObject> lstUnit { get; set; }
        public List<DropdownObject> lstField { get; set; }
        public List<DropdownObject> lstIndividual { get; set; }
        public List<DropdownObject> lstBusiness { get; set; }
        public List<DropdownObject> lstHashTag { get; set; }
    }
	public class RecommendationInsertRequest
    {
        public long? UserId { get; set; }
        public string UserFullName { get; set; }
        public MRRecommendationInsertIN Data { get; set; }
        public List<CAHashtagUpdateIN> ListHashTag { get; set; }
        public List<MRRecommendationFiles> LstXoaFile { get; set; }
        public IFormFileCollection Files { get; set; }
    }
}