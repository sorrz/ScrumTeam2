﻿using HtmlAgilityPack;
using ShopGeneral.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShopGeneral.Services
{
    public class ReportService : IReportService
    {
       
        public string JsonReport(List<Product> products)
        {
            var newtonCompleteJson = JsonConvert.SerializeObject(new { Products = products, Total = products.Count, Skip = 0m, Limit = 0 });

            return newtonCompleteJson;
        }

        public string JsonReport(List<string> strings)
        {
            var newtonCompleteJson = JsonConvert.SerializeObject(new { strings });
            return newtonCompleteJson;
        }

        public string JsonReport(List<Category> categories)
        {
            var newtonCompleteJson = JsonConvert.SerializeObject(new { UnusedCategories = categories });
            return newtonCompleteJson;
        }


    }
}
