using AutoMapper.Configuration.Conventions;
using ShopGeneral.Data;


namespace ShopGeneral.Services
{
    public interface IReportService
    {
        public string JsonReport(List<Product> products);
        public string JsonReport(List<string> strings);
        public string JsonReport(List<int> Ids);
        public string JsonReport(List<Category> categories);
    }
}
