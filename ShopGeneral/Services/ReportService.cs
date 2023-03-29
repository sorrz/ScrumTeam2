using HtmlAgilityPack;
using ShopGeneral.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopGeneral.Services
{
    public class ReportService : IReportService
    {
        public string JsonProductReport(List<Product> products)
        {

            var json = JsonSerializer.Serialize<List<ShopGeneral.Data.Product>>(products);
            var data = JsonSerializer.Serialize(new { Total = products.Count, Skip = 0m, Limit = 0 });
            var complete = JsonSerializer.Serialize(json + data);
            return complete;
        }

        /*
          "total": 100,
          "skip": 0,
          "limit": 30
        */

    }
}
