using HtmlAgilityPack;
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
        public string JsonProductReport(List<Product> products)
        {
            var newtonCompleteJson = JsonConvert.SerializeObject(new { Products = products, Total = products.Count, Skip = 0m, Limit = 0 });

            return newtonCompleteJson;
        }

    }
}
