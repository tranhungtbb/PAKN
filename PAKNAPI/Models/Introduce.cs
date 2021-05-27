using PAKNAPI.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models
{
    public class IntroduceModel
    {
        public SYIntroduce model {get; set;}

        public List<SYIntroduceFunction> lstIntroduceFunction { get; set; }

        //public List<SYIntroduceUnit> lstIntroduceUnit { get; set; }
    }
}
