using ShopGeneral.Services;
﻿using Bogus;
using Humanizer;
using ShopGeneral.Data;
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
        private readonly IFileOutputService _fileOutputService;

        public Product(IProductService productService, IReportService reportService, IFileOutputService fileOutputService)
        {
            _productService = productService;
            _reportService = reportService;
            _fileOutputService = fileOutputService;
        }

        public void Export()
        {
            var listOfProducts = _productService.GetAllProductsOrDefault();
            var report = _reportService.JsonReport(listOfProducts);

            var folderName = "pricerunner";
            var fileName = "";

            _fileOutputService.FileOutput(report, folderName, fileName);
        }
        public void ExportXML(string to)
        {
            var listOfProducts = _productService.GetAllProductsOrDefault();
            // Report Service List of Products -> List of Strings
            var strings = _reportService.productToStringList(listOfProducts);
            var xmlExport = _productService.JsonToXml(strings);
            var report = _reportService.JsonReport(xmlExport);


            var folderPath = Path.Combine("outfiles", to);

            var fullFilePath = Path.Combine(folderPath, DateTime.Now.ToString("yyyyMMdd") + ".xml");

            Directory.CreateDirectory(folderPath);


            using (StreamWriter streamWriter = new StreamWriter(fullFilePath))
            {
                streamWriter.Write(report);
            }

        }

        public void Verifyimage()
        {
            var listOfMissingImages = _productService.VerifyProductImages();
            var report = _reportService.JsonReport(listOfMissingImages.Result);

            var folderName = "products";
            var fileName = "missingimages-";

            _fileOutputService.FileOutput(report, folderName, fileName);
        }

    }
}
