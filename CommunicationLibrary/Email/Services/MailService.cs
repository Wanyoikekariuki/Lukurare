using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using MailKit;
using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using System;
using System.Collections.Generic;

//using EmailSender.Interface;
//using EmailSender.Model;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Options;
//using MimeKit;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Threading.Tasks;
//using Newtonsoft.Json;
//using Microsoft.AspNetCore.Mvc;


public class MailService : IMailService
{
    private readonly MailSettings _mailSettings;

    public MailService(IOptions<MailSettings> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }

    public async Task SendEmailAsync(MailRequest mailRequest)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
        email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
        email.From.Add(MailboxAddress.Parse(mailRequest.From));
        email.Subject = mailRequest.Subject;

        // Log CC email addresses for debugging
        Console.WriteLine($"CC Email Addresses: {mailRequest.CcEmails}");


        // Create a list to hold CC recipients
        var ccRecipients = new List<MailboxAddress>();

        ////// Split the comma-separated CC emails and add them to the list
        if (!string.IsNullOrEmpty(mailRequest.CcEmails))
        {
            var ccEmails = mailRequest.CcEmails.Split(',');
            foreach (var ccEmail in ccEmails)
            {
                ccRecipients.Add(MailboxAddress.Parse(ccEmail.Trim()));
            }
        }

        ////// Add CC recipients to the email
        email.Cc.AddRange(ccRecipients);

        var builder = new BodyBuilder();
        if (mailRequest.Attachments != null)
        {
            byte[] fileBytes;
            foreach (var file in mailRequest.Attachments)
            {
                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                    builder.Attachments.Add(
                        file.FileName,
                        fileBytes,
                        ContentType.Parse(file.ContentType)
                    );
                }
            }
        }
        builder.HtmlBody = mailRequest.Body;
        email.Body = builder.ToMessageBody();
        using var smtp = new SmtpClient();
        smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.Auto);
        smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
        await smtp.SendAsync(email);
        smtp.Disconnect(true);
    }
    //public Task SendEmailAsync(MailRequest mailRequest)
    //{
    //    throw new System.NotImplementedException();
    //}
}
