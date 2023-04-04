using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopGeneral.Data
{
    public class ManufacturerSalesReport
    {
        public Manufacturer _manufacturer { get; set; }
        public BodyBuilder _builder { get; set; }
    }
}
