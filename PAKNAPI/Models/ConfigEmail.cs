using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models
{
    public class ConfigEmail
    {
        public string email { get; set; }
        public string password { get; set; }
        public string server { get; set; }
        public int port { get; set; }
    }
}
