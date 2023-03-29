using ShopGeneral.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopGeneral.Services
{
    public class ReportService : IReportService
    {
        public string JsonProductReport(List<Product> products)
        {
            return "HelloWorld";
        }
    }
}
