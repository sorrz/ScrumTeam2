﻿using AutoFixture;
using AutoMapper;
using Microsoft.Data.Sqlite;
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
        private ApplicationDbContext context;
        private Mock<IMapper> _mapper;
        private Mock<IPricingService> _pricingService;

        public ProductServiceTests()
        {
            _mapper = new Mock<IMapper>();
            _pricingService = new Mock<IPricingService>();
        }


        [TestInitialize]
        public void Init()
        {
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
        public void Should_Return_Correct_Count()
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


        
    }
}
