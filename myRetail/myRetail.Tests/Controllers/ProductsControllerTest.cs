using Microsoft.VisualStudio.TestTools.UnitTesting;
using myRetail.Controllers;
using myRetail.Models;
using System.Collections.Generic;
using System.Linq;

namespace myRetail.Tests.Controllers
{
    [TestClass]
    public class ProductsControllerTest
    {
        //get all products
        [TestMethod]
        public void Get()
        {
            ProductsController controller = new ProductsController();
            
            IEnumerable<Product> result = controller.Get();
            
            Assert.AreNotEqual(0, result.Count());
        }

        //gets the name from the external API if database currently isn't populated.
        [TestMethod]
        public void GetNameFromId()
        {
            // Arrange
            ProductsController controller = new ProductsController();

            // Act
            string result = controller.GetProductName(13860428);

            // Assert
            Assert.AreEqual(result, "The Big Lebowski (Blu-ray)");
        }

        //get the name returns null when the product isn't found
        [TestMethod]
        public void GetNameFromIdFail()
        {
            ProductsController controller = new ProductsController();
            
            string result = controller.GetProductName(0);
            
            Assert.IsNull(result);
        }

        //gets the price from the external API if database currently isn't populated.
        [TestMethod]
        public void GetPriceFromId()
        {
            ProductsController controller = new ProductsController();
            
            string result = controller.GetProductPrice(13860428);
            
            Assert.AreEqual(result, "19.98");
        }

        //get the price returns null when the product isn't found
        [TestMethod]
        public void GetPriceFromIdFail()
        {
            ProductsController controller = new ProductsController();
            
            string result = controller.GetProductPrice(0);
            
            Assert.IsNull(result);
        }

        //get product based on id
        [TestMethod]
        public void GetById()
        {
            ProductsController controller = new ProductsController();
            
            Product result = controller.Get(13860428);
            
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, 13860428);
            Assert.AreEqual(result.Prices[0].Value, 19.98);
            Assert.AreEqual(result.Name, "The Big Lebowski (Blu-ray)");
        }

        //get product returns null when no product found
        [TestMethod]
        public void GetByIdFail()
        {
            ProductsController controller = new ProductsController();

            Product result = controller.Get(0);

            Assert.IsNull(result);
        }

        //updates the price of a product
        [TestMethod]
        public void Put()
        {
            ProductsController controller = new ProductsController();
            
            controller.Put(13860428, 10.00);

            Product result = controller.Get(13860428);
            
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, 13860428);
            Assert.AreEqual(result.Prices[0].Value, 10.00);
            Assert.AreEqual(result.Name, "The Big Lebowski (Blu-ray)");
        }

        //updates the price returns null when no item found
        [TestMethod]
        public void PutFail()
        {
            ProductsController controller = new ProductsController();

            controller.Put(13860428, 10.00);

            Product result = controller.Get(0);

            Assert.IsNull(result);
        }
    }
}
