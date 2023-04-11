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
