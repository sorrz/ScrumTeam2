using ShopGeneral.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAdmin.Commands
{
    public class Categorie : ConsoleAppBase
    {
        private readonly IProductService _productService;
        private readonly IReportService _reportService;

        public Categorie(IProductService productService, IReportService reportService)
        {
            _productService = productService;
            _reportService = reportService;
        }

        public void Checkempty()
        {
            var listOfProducts = _productService.CheckCategories();

            var report = _reportService.JsonProductReport(listOfProducts);


            var folderPath = Path.Combine("outfiles\\category\\");
            var fullFilePath = Path.Combine(folderPath, "missingproducts-", DateTime.Now.ToString("yyyyMMdd") + ".txt");
            Directory.CreateDirectory(folderPath);

            using (StreamWriter streamWriter = new StreamWriter(fullFilePath))
            {
                streamWriter.Write(report);
            }

        }
    }
}
