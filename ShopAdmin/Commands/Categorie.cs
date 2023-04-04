using ShopGeneral.Services;


namespace ShopAdmin.Commands
{
    public class Categorie : ConsoleAppBase
    {
        private readonly IProductService _productService;
        private readonly IReportService _reportService;
        private readonly IFileOutputService _fileOutputService;

        public Categorie(IProductService productService, IReportService reportService, IFileOutputService fileOutputService)
        {
            _productService = productService;
            _reportService = reportService;
            _fileOutputService = fileOutputService;
        }

        public void Checkempty()
        {
            var result = _productService.CheckCategories();
            var report = _reportService.JsonReport(result);

            var folderName = "category";
            var fileName = "missingproducts-";

            _fileOutputService.FileOutput(report, folderName, fileName);
        }
    }
}
