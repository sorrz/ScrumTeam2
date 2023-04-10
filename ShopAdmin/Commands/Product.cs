﻿using ShopGeneral.Services;
﻿using Bogus;
using Humanizer;
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

            var report = _reportService.JsonReport(listOfProducts);


            var folderPath = Path.Combine("outfiles", to);

            var fullFilePath = Path.Combine(folderPath, DateTime.Now.ToString("yyyyMMdd") + ".txt");

            Directory.CreateDirectory(folderPath);


            using (StreamWriter streamWriter = new StreamWriter(fullFilePath))
            {
                streamWriter.Write(report);
            }

        }
        public void ExportXML(string to)
        {
            var listOfProducts = _productService.GetAllProductsOrDefault();
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
            //Need to return to for testing and probably adjust/change after the VerifiProductImages() and such is done.
            var listOfMissingImages = _productService.VerifyProductImages();
            var report = _reportService.JsonReport(listOfMissingImages.Result);

            var folderPath = Path.Combine("outfiles", "products");
            var fullFilePath = Path.Combine(folderPath, "missingimages-" + DateTime.Now.ToString("yyyyMMdd") + ".txt");

            Directory.CreateDirectory(folderPath);

            using (StreamWriter streamWriter = new StreamWriter(fullFilePath))
            {
                streamWriter.Write(report);
            }

        }

        public void  Thumbnail(string folder) // --folder=c:\temp\bilder
        {
            var listOfProduts = _productService.GetAllProductsOrDefault();
            var listOfImageURLs = listOfProduts.Select(e => e.ImageUrl).ToList();

            foreach (var imageURL in listOfImageURLs)
            {
                var thumbnail = _productService.CreateThumbnail(imageURL);
                string fileName = System.IO.Path.Combine(folder, $"{imageURL}" + ".png");
                thumbnail.Save(fileName);
            }

        }

        //public void VerifyimageTest()
        //{
        //    var faltyImageProducts = _productService.VerifyProductImages();

        //    var folderPath = Path.Combine("outfiles", "products");

        //    var fullFilePath = Path.Combine(folderPath, "missingimages-" + DateTime.Now.ToString("yyyyMMdd") + ".txt");

        //    Directory.CreateDirectory(folderPath);

        //    using (StreamWriter streamWriter = new StreamWriter(fullFilePath))
        //    {
        //        foreach (var product in faltyImageProducts.Result)
        //        {
        //            streamWriter.WriteLine(product);
        //        }
        //    }
        //}
    }
}
