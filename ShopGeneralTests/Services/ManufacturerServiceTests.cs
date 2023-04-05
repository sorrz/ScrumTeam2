using AutoFixture;
using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShopAdmin.Configuration;
using ShopAdmin.Services;
using ShopGeneral.Data;
using ShopGeneral.Services;

namespace ShopGeneralTests.Services
{
    [TestClass]
    public class ManufacturerServiceTests
    {
        // Fields used for the testClass
        #region Fields
        //private MailService _sut;
        private ApplicationDbContext context;
        private Mock<IMapper> _mapper;
        private Mock<IPricingService> _pricingService;
        private Mock<HttpMessageHandler> _msgHandler;
        public static HttpMessageHandler _handler;
        private IManufacturerService _sut;
        public Mock<IOptions<MailSettings>> _options;

        #endregion
        // Initializer for the Test Class
        #region Init

        [TestInitialize]
        public void Init()
        {
            _options = new Mock<IOptions<MailSettings>>();
            //_manufacturerService = new Mock<IManufacturerService>();
            _pricingService = new Mock<IPricingService>();

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;
            context = new ApplicationDbContext(contextOptions);
            context.Database.EnsureCreated();

            _sut = new ManufacturerService(context, _pricingService.Object);
        }
        #endregion

        #region Test
        [TestMethod]
        public void GetManufacturerSalesReport_Should_Return_Email_From_Manufacutrers()
        {
            Fixture fixture = new Fixture();
            Product p1 = fixture.Create<Product>();
            p1.Manufacturer.Name = "Leo'S lådbilar";
            p1.Manufacturer.Id = 1;
            p1.Manufacturer.EmailReport = "leo@ladbilar.se";
            context.Products.Add(p1);

            ManufacturerSalesReport m1 = fixture.Create<ManufacturerSalesReport>();
            m1._textBody = "";
            m1._htmlBody = "";
            m1._manufacturer = p1.Manufacturer;


            context.SaveChanges();

            var result = _sut.GetManufacturerSalesReport();

            Assert.AreEqual(p1.Manufacturer.EmailReport.ToString(), result[0]._manufacturer.EmailReport.ToString());

            


        }

        #endregion
    }
}
