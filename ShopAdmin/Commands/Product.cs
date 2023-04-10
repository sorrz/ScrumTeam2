using ShopGeneral.Services;



namespace ShopAdmin.Commands
{
    public class Product : ConsoleAppBase
    {
        private readonly IProductService _productService;
        private readonly IReportService _reportService;

        public Product(IProductService productService, IReportService reportService)
        {
            _productService = productService;
            _reportService = reportService;
        }

        public void Export(string to) // Command is called using "product export --to=pricerunner"
        {
            var listOfProducts = _productService.GetAllProductsOrDefault();

            var report = _reportService.JsonReport(listOfProducts);


            var folderPath = Path.Combine("outfiles", to);

            var fullFilePath = Path.Combine(folderPath, DateTime.Now.ToString("yyyyMMdd") + ".txt");

            Directory.CreateDirectory(folderPath);


            using (StreamWriter streamWriter = new StreamWriter(fullFilePath))
            {
                streamWriter.Write(report);
            }

        }
        public void ExportXML(string to) // Command is called using "product exportxml --to=pricerunner"
        {
            var listOfProducts = _productService.GetAllProductsOrDefault();
            var strings = _reportService.productToStringList(listOfProducts);
            var xmlExport = _productService.JsonToXml(strings);
            var report = _reportService.JsonReport(xmlExport);


            var folderPath = Path.Combine("outfiles", to);

            var fullFilePath = Path.Combine(folderPath, DateTime.Now.ToString("yyyyMMdd") + ".xml");

            Directory.CreateDirectory(folderPath);


            using (StreamWriter streamWriter = new StreamWriter(fullFilePath))
            {
                streamWriter.Write(report);
            }

        }

        public void Verifyimage()
        {
            var listOfMissingImages = _productService.VerifyProductImages();
            var report = _reportService.JsonReport(listOfMissingImages.Result);

            var folderPath = Path.Combine("outfiles", "products");
            var fullFilePath = Path.Combine(folderPath, "missingimages-" + DateTime.Now.ToString("yyyyMMdd") + ".txt");

            Directory.CreateDirectory(folderPath);

            using (StreamWriter streamWriter = new StreamWriter(fullFilePath))
            {
                streamWriter.Write(report);
            }

        }

        public void Thumbnail(string folder) // Command is called using "product thumbnail --folder=c:\temp\bilder"
        {
            var listOfUrls = _productService.GetAllProductsOrDefault()
                .Select(e => e.ImageUrl).ToList();
            
            var i = 1;
            foreach (var imageURL in listOfUrls)
            {
                var image = _productService.GetImageFromUrl(imageURL);
                if (image == null) continue; 
                var thumbnail = _productService.ResizeImage(image, new System.Drawing.Size(100, 100));
                string fileName = Path.Combine(folder, $"image{i}" + ".png");
                thumbnail.Save(fileName);
                i++;
            }

        }

    }
}
