using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PAKNAPI.Common
{
    public static class ConstantRegex
    {
        public const string PHONE = @"^(84|0[3|5|7|8|9])+([0-9]{8})$";

        //min 6 kí tự bao gồm cả chữ và số
        public const string PASSWORD = @"^(?=.*[A-Za-z])(?=.*[0-9])[A-Za-z\d@$!%*#?&()^]{6,}$";

        // cmt và hộ chiếu
        public const string CMT = @"^(\w{1}[0-9]{7}|[0-9]{8}|[0-9]{9}|[0-9]{12})$";

        public const string NUMBER = @"^[0-9]+$";


        public static bool EmailIsValid(string emailAddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailAddress);
                return true;
                
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool CheckValueIsNull(string value) {
            if (value == null || value.Trim() == "") {
                return false;
            }
            return true;
        }
    }

    
}
