
﻿using Bogus.DataSets;
using ShopGeneral.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

﻿using ShopGeneral.Data;



namespace ShopGeneral.Services
{
    public interface IReportService
    {

        public string JsonProductReport(List<Product> products);
        public string JsonReport(List<int> images);

        public string JsonReport(List<Product> products);
        public string JsonReport(List<string> strings);

        public string JsonReport(List<Category> categories);

    }
}
