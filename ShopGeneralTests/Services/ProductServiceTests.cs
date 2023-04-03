using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoFixture;
using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using ShopGeneral.Data;
using ShopGeneral.Services;
using Moq.Protected;

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

        public ProductServiceTests()
        {
            _mapper = new Mock<IMapper>();
            _pricingService = new Mock<IPricingService>();
        }


        [TestInitialize]
        public void Init()
        {
            _msgHandler = new Mock<HttpMessageHandler>();

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
        public void VerifyProductImagesTest()
        {
            //ARRANGE
            var mockProtected = _msgHandler.Protected();





            //ASSERT
            Assert.Fail();

        }





        /* UnitTestP1 ideas */

        //Not create and send a file to Pricerunner?
        //What if there was products in stock before, send a remove-file to Pricerunner?
        [TestMethod]
        public void If_ProductList_Contain_No_Products_Return_Error_No_Products_In_Database() //...And_No_File_Created ?
        {
            //ARRANGE

            //ACT

            //ASSERT

        }
        //Also possible for us to add a function of stock, for example send a list to Pricerunner with no stock.
        //Meaning that we have products in our list and send it but no stock available.


        //Make a testproject for MvSuperShop to have this for testing the outcome of the commands? See the next testmethod as well.
        [TestMethod]
        public void If_File_Is_Empty_Return_Error_File_Is_Empty()
        {
            //ARRANGE

            //ACT

            //ASSERT

        }


        //This could work for P1-P3 if we refactor our code for filecreation.
        //Make a testproject for MvSuperShop to have this for testing the outcome of the commands?
        [TestMethod]
        public void If_File_Has_Not_Been_Created_Should_Return_Error_No_File_Created()
        {
            //ARRANGE

            //ACT

            //ASSERT

        }







    }
}
