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
        private readonly IManufacturerService _manufacturerService;
        private readonly IReportService _reportService;

        public Manufacturer(IMailService mailService, IManufacturerService manufacturerService, IReportService reportService)
        {
            _mailService = mailService;
            _manufacturerService = manufacturerService;
            _reportService = reportService;
        }

        public void Sendreport()
        {
            var manufacturersSalesReports = _manufacturerService.GetManufacturerSalesReport();

            CancellationToken ct = default(CancellationToken);

            manufacturersSalesReports.ForEach(
                m => _mailService.SendAsync(
                    new MailData(
                        new List<string>() { m._manufacturer.EmailReport },
                        "Sales Report for: " + m._manufacturer.Name,
                        m._builder
                        ), ct)
                );

            Console.ReadKey(true); // Waiting for input before closing...
        }
    }
}
