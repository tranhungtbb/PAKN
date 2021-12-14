using Microsoft.AspNetCore.Http;
using PAKNAPI.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models
{
    public class IndexSettingModel
    {
        public SYIndexSetting model {get; set;}

        public List<SYIndexSettingBanner> lstIndexSettingBanner { get; set; }

        public List<SYIndexWebsite> lstSYIndexWebsite { get; set; }

        public List<SYIndexSettingBanner> lstRemoveBanner { get; set; }
        public IFormFileCollection Files { get; set; }

        public IndexSettingModel() { }
    }
}
