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
        public string _textBody { get; set; }
        public string _htmlBody { get; set; }
    }
}
