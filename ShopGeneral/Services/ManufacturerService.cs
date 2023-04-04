using ShopGeneral.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;

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
            foreach (var manufacturer in _context.Manufacturers)
            {
                var builder = new BodyBuilder();

                //MailContext
                var productCountForManufacturer = _context.Products.Where(p => p.Manufacturer.Id == manufacturer.Id).Count();
                var manufacturerName = manufacturer.Name;
                var totalSalesPlaceholder = "XXXX tkr";

                builder.TextBody =  $"Sales report for: {manufacturerName}. " +
                                    $"Total sum of products in our shop: {productCountForManufacturer}. " +
                                    $"Sales total: {totalSalesPlaceholder}.";

                builder.HtmlBody =  $"<h2>Sales report for: {manufacturerName}</h2><br />" +
                                    $"<p>Total sum of products in our shop: {productCountForManufacturer}. " +
                                    $"Sales total: {totalSalesPlaceholder}.</p>";

                var manufacturerSalesReport = new ManufacturerSalesReport() {_manufacturer = manufacturer, _builder = builder};
                manufacturerSalesReports.Add(manufacturerSalesReport);
            }
            return manufacturerSalesReports;
        }
    }
}
