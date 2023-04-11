using MimeKit;
using ShopGeneral.Data;

namespace ShopGeneral.Services
{
    public interface IManufacturerService
    {
        public List<ManufacturerSalesReport> GetManufacturerSalesReport();
    }
}