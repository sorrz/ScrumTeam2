using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using ShopAdmin.Configuration;
using ShopAdmin.Data;
using System.Threading;
using System;


namespace ShopAdmin.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _settings;

        public MailService(IOptions<MailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<bool> SendAsync(MailData mailData, CancellationToken ct)
        {
            try
            {
                // Create a MimeMessage
                var mail = new MimeMessage();

                // Sender
                mail.From.Add(new MailboxAddress(_settings.DisplayName, mailData.From ?? _settings.From));
                mail.Sender = new MailboxAddress(mailData.DisplayName ?? _settings.DisplayName, mailData.From ?? _settings.From);

                // Receiver
                foreach (string mailAddress in mailData.To)
                    mail.To.Add(MailboxAddress.Parse(mailAddress));

                // Try Parse not add un-parsable mail-adresses from the Mail.TO 



                // Set Reply to if specified in mail data
                //if (!string.IsNullOrEmpty(mailData.ReplyTo))
                //    mail.ReplyTo.Add(new MailboxAddress(mailData.ReplyToName, mailData.ReplyTo));

                // Add Content to Mime Message
                var body = mailData.Body;
                mail.Subject = mailData.Subject;
                mail.Body = body.ToMessageBody();

                using var smtp = new SmtpClient();

                if (_settings.UseSSL)
                {
                    await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.SslOnConnect, ct);
                }
                else if (_settings.UseStartTls)
                {

                    //// ASYNC IMPLEMENTATION FROM https://learn.microsoft.com/en-us/answers/questions/226811/mailkit-wont-authenticate-when-trying-to-send-emai
                    //await smtp.ConnectAsync(_settings.Host, 587, SecureSocketOptions.StartTls).ConfigureAwait(true);
                    //await smtp.AuthenticateAsync(_settings.UserName, _settings.Password, ct).ConfigureAwait(true);
                    //await smtp.SendAsync(mail, ct).ConfigureAwait(true);





                    // ---- THIS WORKS BUT IS NOT ASYNC

                    smtp.Connect(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, ct);
                    ////await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, ct);
                    smtp.Authenticate(_settings.UserName, _settings.Password, ct);
                    smtp.Send(mail, ct);
                }

                smtp.Disconnect(true, ct);


                return true;

            }
            catch (Exception)
            {
                return false;
            }



        }
    }
}
