using ShopGeneral.Services;


namespace ShopAdmin.Commands
{
    public class Categorie : ConsoleAppBase
    {
        private readonly IProductService _productService;
        private readonly IReportService _reportService;

        public Categorie(IProductService productService, IReportService reportService)
        {
            _productService = productService;
            _reportService = reportService;
        }

        public string Checkempty()
        {
            var result = _productService.CheckCategories();
            var report = _reportService.JsonReport(result);

            var folderName = "category";
            var fileName = "missingproducts-";


            //Lyfta ut nedan:
            var folderPath = Path.Combine("outfiles\\category\\");
            var fullFilePath = Path.Combine(folderPath, "missingproducts-" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
            Directory.CreateDirectory(folderPath);

            using (StreamWriter streamWriter = new StreamWriter(fullFilePath))
            {
                streamWriter.Write(report);
            }
            //Lyfta ut ovan:


            return result + report + folderName + fileName;
            //return eller ta ovanstående och kalla funktionen härifrån.
        }
    }
}
