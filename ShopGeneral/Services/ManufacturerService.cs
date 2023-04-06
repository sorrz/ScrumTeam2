using ShopGeneral.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using MimeKit.Utils;

namespace ShopGeneral.Services
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPricingService _pricingService;
        public ManufacturerService(ApplicationDbContext context, IPricingService pricingService)
        {
            _context = context;
            _pricingService = pricingService;
        }

        public List<ManufacturerSalesReport> GetManufacturerSalesReport()
        { 
            List<ManufacturerSalesReport> manufacturerSalesReports = new List<ManufacturerSalesReport>();
            foreach (var manufacturer in _context.Manufacturers
                .OrderBy(e => e.EmailReport)
                .Distinct())
            {
                // MailContext
                var productCountForManufacturer = _context.Products.Where(p => p.Manufacturer.Id == manufacturer.Id).Count();
                var imageAdress = _context.Products.Where(p => p.Manufacturer.Id == manufacturer.Id)
                    .Select(m => m.ImageUrl).FirstOrDefault();
                var manufacturerName = manufacturer.Name;
                // Implement Logic for the Sales Revenue from Database when it's added here!
                int? sales = null;
                var totalSalesPlaceholder = $"{sales} tkr";

                var TextBody =  $"Sales report for: {manufacturerName}. " +
                                    $"Total sum of products in our shop: {productCountForManufacturer}. " +
                                    $"Sales total: {totalSalesPlaceholder}.";

                var HtmlBody =  $"<img src='{imageAdress}'> <br />" +
                    $"<h2>Sales report for: {manufacturerName}</h2><br />" +
                                    $"<p>Total sum of products in our shop: {productCountForManufacturer}.<br />" +
                                    $"<br /> " +
                                    $"Sales total: {totalSalesPlaceholder} the last 30 days.</p>";

                var manufacturerSalesReport = new ManufacturerSalesReport() 
                    {_manufacturer = manufacturer, _textBody = TextBody, _htmlBody = HtmlBody};
                manufacturerSalesReports.Add(manufacturerSalesReport);
            }
            return manufacturerSalesReports;
        }
    }
}
