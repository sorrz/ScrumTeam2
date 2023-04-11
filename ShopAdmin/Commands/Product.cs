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


        public void Export(string to) // Command is called using "product export --to=pricerunner"

        {
            var listOfProducts = _productService.GetAllProductsOrDefault();
            var report = _reportService.JsonReport(listOfProducts);

            var folderName = to;
            var fileName = "";
            var fileEnding = ".txt";

            _fileOutputService.FileOutput(report, folderName, fileName, fileEnding);
        }

        public void ExportXML(string to) // Command is called using "product exportxml --to=pricerunner"
        {
            var listOfProducts = _productService.GetAllProductsOrDefault();
            var strings = _reportService.productToStringList(listOfProducts);
            var xmlExport = _productService.JsonToXml(strings);
            var report = _reportService.JsonReport(xmlExport);

            var folderName = to;
            var fileName = "";
            var fileEnding = ".xml";

            _fileOutputService.FileOutput(report, folderName, fileName, fileEnding);
        }

        public void Verifyimage()
        {
            var listOfMissingImages = _productService.VerifyProductImages();
            var report = _reportService.JsonReport(listOfMissingImages.Result);

            var folderName = "products";
            var fileName = $"missingimages-";
            var filEnding = ".txt";

            _fileOutputService.FileOutput(report, folderName, fileName, filEnding);
        }


        public void Thumbnail(string folder) // Command is called using "product thumbnail --folder=c:\temp\bilder"
        {
            var listOfUrls = _productService.GetAllProductsOrDefault()
                .Select(e => e.ImageUrl).ToList();
            
            var i = 1;
            foreach (var imageURL in listOfUrls)
            {
                var image = _productService.GetImageFromUrl(imageURL);
                if (image == null) continue; 
                var thumbnail = _productService.ResizeImage(image, new System.Drawing.Size(100, 100));
                string fileName = Path.Combine(folder, $"image{i}" + ".png");
                thumbnail.Save(fileName);
                i++;
            }

        }
    }
}
