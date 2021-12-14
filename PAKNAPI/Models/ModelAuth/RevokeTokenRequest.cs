using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Model.ModelAuth
{
    public class RevokeTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
