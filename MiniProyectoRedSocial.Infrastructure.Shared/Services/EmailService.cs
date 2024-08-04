﻿using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MiniProyectoRedSocial.Core.Application.Dtos.Email;
using MiniProyectoRedSocial.Core.Application.Interfaces.Services;
using MiniProyectoRedSocial.Core.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProyectoRedSocial.Infrastructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        private MailSettings _mailSettings {  get; }

        public EmailService(IOptions<MailSettings> mailsettings)
        { 
            _mailSettings = mailsettings.Value;
        }
        public async Task SendAsync(EmailRequest request)
        {
            try
            {
                MimeMessage email = new();
                email.Sender = MailboxAddress.Parse($"{_mailSettings.DisplayName} < {_mailSettings.EmailFrom}");
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;
                BodyBuilder builder = new();
                builder.HtmlBody = request.Body;
                email.Body = builder.ToMessageBody();

                using SmtpClient smtp = new();
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort,MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);

            }
            catch(Exception ex)
            {

            }

        }
    }
}
