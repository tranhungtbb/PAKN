using PAKNAPI.Common;
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
    }
}