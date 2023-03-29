using HtmlAgilityPack;
using ShopGeneral.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ShopGeneral.Services
{
    public class ReportService : IReportService
    {
        public string JsonProductReport(List<Product> products)
        {
            var JsonObject = JsonSerializer.Serialize(JsonProductReport);
            //return tidigare här uppe

            var json = JObject.Parse(JsonSerializer.Serialize<List<ShopGeneral.Data.Product>>(products));
            var data = JObject.FromObject(new { Total = products.Count, Skip = 0m, Limit = 0 });
            var complete = new JObject();
            complete.Merge(json);
            complete.Merge(data);
            return complete.ToString();

            return JsonObject;
        }

        /*
          "total": 100,
          "skip": 0,
          "limit": 30
        */

    }
}
