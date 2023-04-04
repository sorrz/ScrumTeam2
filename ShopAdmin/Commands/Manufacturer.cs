using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using Org.BouncyCastle.Asn1.Pkcs;
using ShopAdmin.Data;
using ShopAdmin.Services;
using ShopGeneral.Services;

namespace ShopAdmin.Commands
{
    public class Manufacturer : ConsoleAppBase
    {
        private readonly IMailService _mailService;
        private readonly IProductService _productService;

        public Manufacturer(IMailService mailService, IProductService productService)
        {
            _mailService = mailService;
            _productService = productService;
        }

        public async void Sendreport()
        {
            var to = _productService.GetManufacturerEmails();
            List<string>? bcc = new List<string>();
            List<string>? cc = new List<string>();
            //to.Add("jarod.strosin70@ethereal.email");
            var subject = "HelloWorldSubject";
            string? body = null;
            string? from = null;
            string? displayName = null;
            string? replyTo = null;
            string? replyToName = null;
            CancellationToken ct = default(CancellationToken);
            MailData mailData = new(to, subject, body, from, displayName, replyTo, replyToName, bcc, cc);
            var result = await _mailService.SendAsync(mailData, ct);
            Console.ReadKey(true); // Waiting for input before closing...
        }
    }
}
