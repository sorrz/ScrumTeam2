using ShopGeneral.Services;
﻿using Bogus;
using Humanizer;
using ShopGeneral.Data;
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

            var report = _reportService.JsonReport(listOfProducts);


            var folderPath = Path.Combine("outfiles", to);

            var fullFilePath = Path.Combine(folderPath, DateTime.Now.ToString("yyyyMMdd") + ".txt");

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
            var fullFilePath = Path.Combine(folderPath,"missingimages-" + DateTime.Now.ToString("yyyyMMdd") + ".txt");

            Directory.CreateDirectory(folderPath);

            using (StreamWriter streamWriter = new StreamWriter(fullFilePath))
            {
                streamWriter.Write(report);
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
        //            streamWriter.WriteLine(product.Id);
        //        }
        //    }
        //}
    }
}
