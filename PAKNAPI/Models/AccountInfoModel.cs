using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models
{
    public class AccountInfoModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string DateOfBirth { get;set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Nation { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int WardsId { get; set; }
        public string Address { get; set; }
        public string IdCard { get; set; }
        public string IssuedPlace { get; set; }
        public string IssuedDate { get; set; }
        public bool Gender { get; set; }
    }
}
