using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using ShopGeneral.Data;
using ShopGeneral.Services;


namespace ShopGeneralTests.Services
{
    [TestClass]
    public class ProductServiceTests
    {
        private ProductService _sut;
        private Mock<ApplicationDbContext> _context2;
        private Mock<IMapper> _mapper;
        private Mock<IPricingService> _pricingService;
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;


        public ProductServiceTests()
        {
            _dbContextOptions = new DbContextOptions<ApplicationDbContext>();
            _context2 = new Mock<ApplicationDbContext>(_dbContextOptions);
            _mapper = new Mock<IMapper>();
            _pricingService = new Mock<IPricingService>();
            _sut = new ProductService(_context2.Object, _pricingService.Object, _mapper.Object);
        }

        // https://learn.microsoft.com/en-us/ef/ef6/fundamentals/testing/mocking


        [TestMethod]
        public void Should_Return_Correct_Count()
        {
            //ARR
            var data = new List<Product>
            {
                new Product { Name = "Ford" },
                new Product { Name = "Alpha Romeo" },
                new Product { Name = "Volvo" },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Product>>();
            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            //ACT
            // THROWS ERROR ! System.NotSupportedException: Unsupported expression: c => c.Products
            _context2.Setup(c => c.Products).Returns(mockSet.Object);
            var result = _sut.GetAllProductsOrDefault();
            
            //ASS
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Alpha Romeo", result[0].Name);
            Assert.AreEqual("Ford", result[1].Name);
            Assert.AreEqual("Volvo", result[2].Name);


        }


        
    }
}
