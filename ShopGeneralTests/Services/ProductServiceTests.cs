using AutoFixture;
using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using ShopGeneral.Data;
using ShopGeneral.Services;
using System.Net;
using RichardSzalay.MockHttp;

namespace ShopGeneralTests.Services
{
    [TestClass]
    //[Ignore]
    public class ProductServiceTests
    {
        private ProductService _sut;
        private ApplicationDbContext context;
        private Mock<IMapper> _mapper;
        private Mock<IPricingService> _pricingService;
        //private Mock<HttpMessageHandler> _msgHandler;
        //private static HttpMessageHandler _handler;
        private Mock<IHttpClientFactory> _mockHttpClientFactory;

        [TestInitialize]
        //[Ignore]

        public void Init()
        {

            //_msgHandler = new Mock<HttpMessageHandler>();
            _mapper = new Mock<IMapper>();
            _pricingService = new Mock<IPricingService>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();


            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;
            context = new ApplicationDbContext(contextOptions);
            context.Database.EnsureCreated();

            _sut = new ProductService(context, _pricingService.Object, _mapper.Object, _mockHttpClientFactory.Object);
        }

        [TestMethod]
        //[Ignore]

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

        [TestMethod]
        //[Ignore]

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

        //[TestMethod]
        //[Ignore]

        //public void Any_Image_url_should_return_not_found_and_return_product_id_list()
        //{
        //    //ARRANGE
        //    Fixture fixture = new Fixture();
        //    HttpMessageHandler _handler = fixture.Create<HttpMessageHandler>();
        //    Product p1 = fixture.Create<Product>();
        //    p1.Id = 1;
        //    fixture.Inject(new UriScheme("http"));
        //    p1.ImageUrl = fixture.Create<Uri>().AbsoluteUri;
        //    context.Products.Add(p1);
        //    context.SaveChanges();

        //    //var mockedProtected = _handler.Protected();
        //    var setupHttpRequest = _handler<HttpResponseMessage>>(
        //        "SendAsync",
        //        ItExpr.IsAny<HttpRequestMessage>(),
        //        ItExpr.IsAny<CancellationToken>()
        //        );
        //    var httpMockedResponse =
        //        setupHttpRequest.ReturnsAsync(new HttpResponseMessage()
        //        {
        //            StatusCode = HttpStatusCode.NotFound
        //        });

        //    //ACT
        //    var result = _sut.VerifyProductImages();

        //    //ASSERT
        //    Assert.AreEqual(1, result.Result[0]);

        //}

        //[TestMethod]
        //// https://docs.educationsmediagroup.com/unit-testing-csharp/advanced-topics/testing-httpclient
        //public void Should_return_string_from_URI()
        //{


        //    // ARRANGE
        //    var fixture = new Fixture();
        //    fixture.AddMockHttp();
        //    var testUri = fixture.Create<Uri>().AbsoluteUri;

        //    // GENERATE CONTENT 

        //    Product p1 = fixture.Create<Product>();
        //    p1.Id = 1;
        //    fixture.Inject(new UriScheme("http"));
        //    p1.ImageUrl = fixture.Create<Uri>().AbsoluteUri;
        //    context.Products.Add(p1);
        //    context.SaveChanges();



        //    // MOCK A NEW MESSAGE HANDLER
        //    var handler = new MockHttpMessageHandler();
        //    //handler.When(HttpMethod., testUri.ToString())
        //    //       .Respond(HttpStatusCode.NotFound);

        //    handler.When(ItExpr.IsAny<HttpRequestOptions>).SendAsync(ItExpr.IsAny<HttpRequestMessage>, ItExpr.IsAny<CancellationToken>());



        //    var http = handler.ToHttpClient();

        //    _mockHttpClientFactory.Setup(p => p.CreateClient(It.IsAny<string>())).Returns(http);


        //    // ACT
        //    var result = _sut.VerifyProductImages();

        //    // ASSERT
        //    Assert.AreEqual(1, result.Result[0]);


        public class Service
        {
            private readonly IHttpClientFactory _httpFactory;

            public Service(IHttpClientFactory httpFactory)
            {
                _httpFactory = httpFactory ?? throw new ArgumentNullException(nameof(httpFactory));
            }

            public Task<string> GetStringAsync(Uri uri)
            {
                var http = _httpFactory.CreateClient(nameof(Service));

                return http.GetStringAsync(uri);
            }
        }
        [TestMethod]
        public async Task Should_Return_one_failed_instance_of_image_Verification()
        {
            // ARRANGE
            var fixture = new Fixture();
            var testUri = fixture.Create<Uri>();
            var expectedResult = 1;
            var outString = "hejsan";


            Product p1 = fixture.Create<Product>();
            p1.Id = 1;
            p1.ImageUrl = "http://www.google.se/image00.jpg";
            context.Products.Add(p1);
            context.SaveChanges();


            // Johan, kan vi snygga till det här? 
            // Vi testar det vi vill, men inte riktigt på rätt sätt.
            var handler = new MockHttpMessageHandler();
            handler.When(HttpMethod.Get, testUri.ToString())
                   .Respond(HttpStatusCode.NotFound, new StringContent(outString));

            var http = handler.ToHttpClient();

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(p => p.CreateClient(It.IsAny<string>())).Returns(http);

            var sut = new ProductService(context, _pricingService.Object, _mapper.Object, mockHttpClientFactory.Object);

            // ACT
            var result = await sut.VerifyProductImages();

            // ASSERT
            Assert.AreEqual(expectedResult, result.Count);
        }



    }

}

