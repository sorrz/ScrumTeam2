using Bogus;
using ShopGeneral.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopAdmin.Commands
{
    public class Product : ConsoleAppBase
    {
        private readonly IProductService _productService;
        private readonly IReportService _reportService;

        public Product(IProductService productService, IReportService reportService)
        {
            _productService = productService;
            _reportService = reportService;
        }

        public void Export(string to)
        {
            var listOfProducts = _productService.GetAllProductsOrDefault();

            var report = _reportService.JsonProductReport(listOfProducts);


            var folderPath = Path.Combine("outfiles", to);

            var fullFilePath = Path.Combine(folderPath, DateTime.Now.ToString("yyyyMMdd") + ".txt");

            Directory.CreateDirectory(folderPath);


            using (StreamWriter streamWriter = new StreamWriter(fullFilePath))
            {
                streamWriter.Write(report);
            }

        }
    }
}
