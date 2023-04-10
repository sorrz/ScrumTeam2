using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopGeneral.Data;
using ShopGeneral.Infrastructure.Context;
using System.Net;
using System.Xml;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ShopGeneral.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPricingService _pricingService;
    public static HttpClient? _httpClient;
    public static HttpMessageHandler? _handler { get; set; }

    public ProductService(ApplicationDbContext context, IPricingService pricingService, IMapper mapper)
    {
        _context = context;
        _pricingService = pricingService;
        _mapper = mapper;
    }

    public IEnumerable<ProductServiceModel> GetNewProducts(int cnt, CurrentCustomerContext context)
    {
        return _pricingService.CalculatePrices(_mapper.Map<IEnumerable<ProductServiceModel>>(_context.Products
            .Include(e => e.Category)
            .Include(e => e.Manufacturer)
            .OrderByDescending(e => e.AddedUtc)
            .Take(cnt)), context);
    }

    public List<Product> GetAllProductsOrDefault() => _context.Products.OrderBy(x => x.Name).ToList();

    public async Task<List<int>> VerifyProductImages()
    {
        var products = _context.Products.ToList();
        List<int> productImageNotFound = new();
        _httpClient = new HttpClient();

        if (_handler is not null)
        {
            _httpClient = new HttpClient(_handler);
        }

        foreach (var product in products)
        {
            try
            {
                using (var response = await _httpClient.GetAsync(product.ImageUrl))
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                        productImageNotFound.Add(product.Id);
                }

            }
            catch (Exception)
            {
                throw;
            }

        }
        return productImageNotFound;
    }

    public List<Category> CheckCategories()
    {
        var categoryList = _context.Categories.OrderBy(y => y.Name).ToList();
        var productList = _context.Products.OrderBy(x => x.Category.Name).ToList();
        var result = categoryList.ExceptBy(productList
            .Select(a => a.Category.Name), x => x.Name).ToList();
        return result;
    }

    public List<string> JsonToXml(List<string> JsonInput)
    {
        List<string> xmlString = new();
        XmlDocument doc = new();

        foreach (var item in JsonInput)
        {
            doc = JsonConvert.DeserializeXmlNode(item);
            xmlString.Add(doc.InnerXml.ToString());
        }

        return xmlString;
    }

    public System.Drawing.Image GetImageFromUrl(string urlInput)
    {

        System.Drawing.Image image = null;

        try
        {

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(urlInput);
            webRequest.AllowWriteStreamBuffering = true;
            webRequest.Timeout = 30000;

            WebResponse webResponse = webRequest.GetResponse();
            Stream stream = webResponse.GetResponseStream();
            image = Image.FromStream(stream);
            webResponse.Close();
        }
        catch (Exception ex)
        {
            return null;
        }
        return image;
    }

    public Image ResizeImage(Image image, Size size)
    {
        // Entry Input
        var sourceWidth = image.Width;
        var sourceHeight = image.Height;
        float percentX = 0;
        var percentW = (float)size.Width / sourceWidth;
        var percentH = (float)size.Height / sourceHeight;
        // Calc for new desired Size
        if (percentH < percentW)
            percentX = percentH;
        else
            percentX = percentW;

        // New output 
        int destWidth = (int)(sourceWidth * percentX);
        int destHeight = (int)(sourceHeight * percentX);

        Bitmap thumbnail = new Bitmap(destWidth, destHeight);
        Graphics g = Graphics.FromImage(thumbnail);
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;

        // Draw Image to Thumbnail Var
        g.DrawImage(image, 0, 0, destWidth, destHeight);
        g.Dispose();

        return thumbnail;
    }
}

