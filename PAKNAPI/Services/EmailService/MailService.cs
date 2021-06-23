using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using PAKNAPI.Services.FileUpload;

namespace PAKNAPI.Services.EmailService
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly IFileService _fileService;
        //private readonly IWebHostEnvironment _webHostEnvironment;
        public MailService(MailSettings mailSettings, IWebHostEnvironment webHostEnvironment)
        {
            _mailSettings = mailSettings;
            //_webHostEnvironment = webHostEnvironment;
            _fileService = new FileService(webHostEnvironment);
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Email);

            InternetAddressList listEmailAddress = new InternetAddressList();
            foreach(var emailAddress in mailRequest.ToEmails)
            {
                listEmailAddress.Add(MailboxAddress.Parse(emailAddress));
            }

            email.To.AddRange(listEmailAddress);
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (System.IO.File.Exists(file))
                    {
                        using (FileStream fileStream = new FileStream(file, FileMode.Open))
                        using (var memoryStream = new MemoryStream())
                        {
                            await fileStream.CopyToAsync(memoryStream);
                            fileBytes = memoryStream.ToArray();

                            var filename = fileStream.Name.Substring(15);
                            builder.Attachments.Add(filename, fileBytes, ContentType.Parse(fileBytes));
                        }
                    }
                    
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Server, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
