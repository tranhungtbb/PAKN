using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Model.ModelAuth
{
    /// <example>
    /// { 
    ///		"RefreshToken" : "QDX6/o3fmf77dWW1MvY7MX64ghTuBQh7fmFQlaizD6w5FXBv8i805DfXiC246baH7E1ls+tZFdrqi6DTakKZtw=="
    /// }
    /// </example>
    public class RevokeTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
