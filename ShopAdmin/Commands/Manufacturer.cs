using ShopGeneral.Services;


namespace ShopAdmin.Commands
{

    public class Manufacturer : ConsoleAppBase
    {
        private readonly IProductService _productService;
        private readonly IReportService _reportService;

        public Manufacturer(IProductService productService, IReportService reportService)
        {
            _productService = productService;
            _reportService = reportService;
        }

        public void Sendreport()
        {
            // loopa alla Manufacturers, exportera emails till List
            var manufacuturerEmails = _productService.GetAllManufacturerEmails();
            // anropa en Service som ger er ett HTML-mail
            // Som ska "skickas" via https://ethereal.email/
            // den skickar ju inte vidare till rätt mottagare utan lagrar den så ni kan kolla

            Console.ReadKey(true);


            //var result = _productService.CheckCategories();
            //var report = _reportService.JsonReport(result);


            //var folderPath = Path.Combine("outfiles\\category\\");
            //var fullFilePath = Path.Combine(folderPath, "missingproducts-" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
            //Directory.CreateDirectory(folderPath);

            //using (StreamWriter streamWriter = new StreamWriter(fullFilePath))
            //{
            //    streamWriter.Write(report);
            //}

        }
    }


}
