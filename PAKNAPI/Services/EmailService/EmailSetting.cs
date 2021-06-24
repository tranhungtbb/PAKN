using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Services.EmailService
{
    public class EmailSetting
    {
    }
    public class MailSettings
    {
        //{"email":"tranhung110398123@gmail.com","password":"130298110395","server":"smtp.gmail.com","port":"587"}
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
    }
    public class MailRequest
    {
        public IEnumerable<string> ToEmails { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        //public List<IFormFile> Attachments { get; set; }
        public IEnumerable<string> Attachments { get; set; }
    }

}
