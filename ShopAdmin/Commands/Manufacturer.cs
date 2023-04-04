using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1.Pkcs;
using ShopAdmin.Data;
using ShopAdmin.Services;

namespace ShopAdmin.Commands
{
    public class Manufacturer : ConsoleAppBase
    {
        private readonly IMailService _mailService;
        public Manufacturer(IMailService mailService)
        {
            _mailService = mailService;
        }

        public async void Sendreport()
        {
            List<string> to = new List<string>();
            List<string> bcc = new List<string>();
            List<string> cc = new List<string>();
            to.Add("johan.kreivi@gmail.com");
            var subject = "HelloWorldSubject";
            string? body = null;
            string? from = null;
            string? displayName = null;
            string? replyTo = null;
            string? replyToName = null;
            CancellationToken ct = default(CancellationToken);
            MailData mailData = new(to, subject, body, from, displayName, replyTo, replyToName, bcc, cc);
            var result = await _mailService.SendAsync(mailData, ct);
        }
    }
}
