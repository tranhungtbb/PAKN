using Quartz;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System;
using Quartz.Impl;
using System.Net;
using PAKNAPI.ModelBase;
using PAKNAPI.Common;
using Bugsnag;
using Microsoft.AspNetCore.Hosting;
using PAKNAPI.Models;

namespace PAKNAPI.Job
{
    public class MailHelper
    {
        public static bool SendMail(ConfigEmail configEmail ,string toEmail, string subject, string content, List<string> files = null)
        {
            try
            {
                var smtpClient = new SmtpClient(configEmail.server, configEmail.port)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(configEmail.email, configEmail.password),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true,
                    Timeout = 100000
                };

                var mail = new MailMessage
                {
                    Body = content,
                    Subject = subject,
                    From = new MailAddress(configEmail.email)
                };
                mail.To.Add(new MailAddress(toEmail));

                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                if (files != null) {
                    foreach (var item in files) {
                        mail.Attachments.Add(new Attachment(item));
                    }
                }
        

                smtpClient.Send(mail);

                return true;
            }
            catch (SmtpException)
            {
                return false;
            }
        }
    }
}