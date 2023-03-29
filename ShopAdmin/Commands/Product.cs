using ShopGeneral.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAdmin.Commands
{
    internal class Product : ConsoleAppBase
    {
        private readonly IProductService _productService;
        private readonly IReportService _reportService;

        public Product(IProductService productService, IReportService reportService)
        {
            _productService = productService;
            _reportService = reportService;
        }

        public void Export(string toFolder)
        {
            var listOfProducts = _productService.GetAllProductsOrDefault();

            var report = _reportService.JsonProductReport(listOfProducts);

            var folderPath = Path.Combine(@"\outfiles\", toFolder, DateTime.Now.Date.ToString(), ".txt");
            using (StreamWriter streamWriter = new StreamWriter(folderPath))
            {
                streamWriter.Write(report);
            }

        }
    }
}
