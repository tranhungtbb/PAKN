using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Chat.ResponseModel
{
    public class User
    {
       
        public int Id { get; set; }       
        public string FullName { get; set; }
        public string userName { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int EnableBot { get; set; }
        public int Role { get; set; }
    }
}
