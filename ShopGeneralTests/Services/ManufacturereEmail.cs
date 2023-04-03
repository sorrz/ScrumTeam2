using ShopGeneral.Data;
using ShopGeneral.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopGeneralTests.Services
{
    [TestClass]
    public class GetAllEmailAddressesTests
    {
        [TestMethod]
        public void RetrieveAllEmailAddresses_ReturnsOnlyEmailAddresses()
        {
            // Arrange
            var manufacturers = new List<Manufacturer>()
            {
                new Manufacturer() { Id = 1, Name = "Manufacturer 1", EmailReport = "email1@example.com" },
                new Manufacturer() { Id = 2, Name = "Manufacturer 2", EmailReport = "" },
                new Manufacturer() { Id = 3, Name = "Manufacturer 3", EmailReport = "email3@example.com" },
                new Manufacturer() { Id = 4, Name = "Manufacturer 4", EmailReport = null }
            };

            // Act
            var result = GetAllEmailAdress.RetrieveAllEmailAddresses(manufacturers);

            // Assert
            CollectionAssert.AreEqual(new List<string>() { "email1@example.com", "email3@example.com" }, result);
        }
    }
}
