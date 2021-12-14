using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Services.EmailService
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
