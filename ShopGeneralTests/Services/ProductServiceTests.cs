using AutoFixture;
using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using ShopGeneral.Data;
using ShopGeneral.Services;
using Moq.Protected;
using System.Net;
using MailKit;
using Newtonsoft.Json;

namespace ShopGeneralTests.Services
{
    [TestClass]
    public class ProductServiceTests
    {
        private ProductService _sut;
        private ApplicationDbContext context;
        private Mock<IMapper> _mapper;
        private Mock<IPricingService> _pricingService;
        private Mock<HttpMessageHandler> _msgHandler;
        public static HttpMessageHandler _handler;
        public Mock<IMailService> _mailService;
        public Mock<IManufacturerService> _manufacturerService;

        [TestInitialize]
        public void Init()
        {
            _msgHandler = new Mock<HttpMessageHandler>();
            _mapper = new Mock<IMapper>();
            _pricingService = new Mock<IPricingService>();
            _mailService = new Mock<IMailService>();
            _manufacturerService = new Mock<IManufacturerService>();

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;
            context = new ApplicationDbContext(contextOptions);
            context.Database.EnsureCreated();

            _sut = new ProductService(context, _pricingService.Object, _mapper.Object);

        }

        


        [TestMethod]
        public void Should_Return_Correct_Count_and_Sorting()
        {
            //ARR
            Fixture fixture = new Fixture();
            Product p1 = fixture.Create<Product>();
            Product p2 = fixture.Create<Product>();
            Product p3 = fixture.Create<Product>();

            p1.Name = "Volvo";
            p2.Name = "Ford";
            p3.Name = "Alpha Romeo";

            context.Products.Add(p1);
            context.Products.Add(p2);
            context.Products.Add(p3);

            context.SaveChanges();

            //ACT
            var result = _sut.GetAllProductsOrDefault();
            
            //ASS
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Alpha Romeo", result[0].Name);
            Assert.AreEqual("Ford", result[1].Name);
            Assert.AreEqual("Volvo", result[2].Name);


        }


        //[TestMethod()]
        //public void VerifyProductImagesTest()
        //{
        //    //ARRANGE
        //    Fixture fixture = new();
        //    Product p1 = fixture.Create<Product>();

        //    p1.Id = 404;

        //    context.Products.Add(p1);

        //    context.SaveChanges();

        //    //ACT
        //    var result = _sut.VerifyProductImages();

        //    //ASSERT
        //    Assert.AreEqual(1, result.Result.Count);
        //    Assert.AreEqual(404, result.Result[0]);
        //}

        //Flyttad från Product.cs i ShopAdmin:
        //public void VerifyimageTest()
        //{
        //    var faltyImageProducts = _productService.VerifyProductImages();

        //    var folderPath = Path.Combine("outfiles", "products");

        //    var fullFilePath = Path.Combine(folderPath, "missingimages-" + DateTime.Now.ToString("yyyyMMdd") + ".txt");

        //    Directory.CreateDirectory(folderPath);

        //    using (StreamWriter streamWriter = new StreamWriter(fullFilePath))
        //    {
        //        foreach (var product in faltyImageProducts.Result)
        //        {
        //            streamWriter.WriteLine(product.Id);
        //        }
        //    }
        //}


        [TestMethod]
        public void CheckCategories_Should_Return_c3_CategoryName()
        {
            //ARR
            Fixture fixture = new Fixture();
            Category c1 = fixture.Create<Category>();
            Category c2 = fixture.Create<Category>();
            Category c3 = fixture.Create<Category>();
            Product p1 = fixture.Create<Product>();
            Product p2 = fixture.Create<Product>();

            c1.Name = "Van";
            c2.Name = "Pickup";
            c3.Name = "Sladdare";

            p1.Category.Name = "Van";
            p2.Category.Name = "Pickup";

            context.Categories.Add(c1);
            context.Categories.Add(c2);
            context.Categories.Add(c3);
            context.Products.Add(p1);
            context.Products.Add(p2);

            context.SaveChanges();

            //ACT
            var result = _sut.CheckCategories();

            //ASS
            Assert.AreEqual(c3.Name, result[0].Name.ToString());
        }
        
        [TestMethod]
        public void Any_Image_url_should_return_not_found_and_return_product_id_list()
        {

            //ARRANGE

            //var mockProtected = _msgHandler.Protected();

            Fixture fixture = new Fixture();
            Product p1 = fixture.Create<Product>();
            p1.Id = 1;
            p1.ImageUrl = "http://www.google.se/image004.jpg";
            context.Products.Add(p1);
            context.SaveChanges();

            var mockedProtected = _msgHandler.Protected();
            var setupHttpRequest = mockedProtected.Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
                );
            var httpMockedResponse =
                setupHttpRequest.ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound
                });

            //ACT
            var result = _sut.VerifyProductImages();

            //ASSERT
            Assert.AreEqual(1, result.Result[0]);



            //ASSERT
            //Assert.Fail();

        }


        [TestMethod]
        public void If_ProductList_Contain_No_Products_Return_No_Products_In_Database()
        {
            var obj = new Product();
            context.Products.Add(obj);
            context.SaveChanges();

            var result = _sut.GetAllProductsOrDefault();

            Assert.AreEqual(null, result.Count);
        }

        [TestMethod]
        public void Product_in_JSON_Format_Should_Return_a_xml_output()
        {

            //ARRANGE
            Fixture fixture = new Fixture();
            Product p1 = fixture.Create<Product>();
            p1.Id = 1232435;
            context.Products.Add(p1);
            context.SaveChanges();

            var products = new List<Product>();
            products.Add(p1);

            List<string> input = new();
            var newtonCompleteJson = JsonConvert.SerializeObject(new { Products = products });
            input.Add(newtonCompleteJson);

            //ACT
            var result = _sut.JsonToXml(input);



            //ASSERT
            //Assert.IsNotNull(result);
        }

    }
}
