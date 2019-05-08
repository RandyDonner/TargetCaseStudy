using LiteDB;
using myRetail.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Web.Http;

namespace myRetail.Controllers
{
    public class ProductsController : ApiController
    {
        //make one instance and keep using
        private LiteDatabase liteDatabase;
        private LiteDatabase getLiteDatabase()
        {
            if (ReferenceEquals(null, liteDatabase))//create if null
            {
                try
                {
                    liteDatabase = new LiteDatabase(ConfigurationManager.AppSettings["connectionString"]);
                    return liteDatabase;
                }
                catch (ConfigurationErrorsException ex)
                {
                    throw new Exception("Missing the database connection location.");
                }
            }
            return liteDatabase;
        }

        /// <summary>
        /// Get the name of the product from the external API.
        /// </summary>
        /// <param name="id">ID of the product to search.</param>
        /// <returns>The name of the product if found otherwise null.</returns>
        public string GetProductName(int id)
        {
            TargetNameFunctionality name = new TargetNameFunctionality();
            return name.GetName(id);
        }

        /// <summary>
        /// Get the price of the product from the external API.
        /// </summary>
        /// <param name="id">ID of the product to search.</param>
        /// <returns>The price of the product if found otherwise null.</returns>
        public string GetProductPrice(int id)
        {
            TargetNameFunctionality name = new TargetNameFunctionality();
            return name.GetInitialPrice(id);
        }

        // GET api/products
        [HttpGet]
        [Route("api/products")]
        public IEnumerable<Product> Get()
        {
            LiteDatabase db = getLiteDatabase();
            try
            {
                using (db)
                {
                    var collection = db.GetCollection<Product>("Product");
                    return collection.FindAll();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET api/products/5
        [HttpGet]
        [Route("api/products/{id}")]
        public Product Get(int id)
        {
            LiteDatabase db = getLiteDatabase();//get database connection
            try
            {
                using (db)
                {
                    var collection = db.GetCollection<Product>("Product");
                    Product product = new Product() { Id = id };//create our product

                    string name = GetProductName(id);//get name from Target API
                    if (ReferenceEquals(null, name))
                    {
                        return null;//Item not found in Target api search
                    }
                    product.Name = name;

                    Product dbProduct = collection.FindById(id);//search our Data Store for a saved product with updated price
                    if (ReferenceEquals(null, dbProduct))
                    {
                        //db was empty so take the initial price from Target API
                        string price = GetProductPrice(id);
                        if (ReferenceEquals(null, price))
                        {
                            return null;//Price not found in Target api search
                        }
                        product.Prices = new List<Price>() { new Price() { Value = double.Parse(price), CurrencyCode = CurrencyCodeTypes.USD } };
                    }
                    else
                    {
                        product.Prices = dbProduct.Prices;
                    }

                    try
                    {
                        collection.Insert(product);//insert to db
                    }
                    catch (LiteException ex)//if duplicate found just return product
                    {
                        return product;
                    }

                    return product;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // PUT api/products/5
        [HttpPut]
        [Route("api/products/putValue/{id}")]
        [ActionName("putValue")]
        public void Put(int id, [FromBody]NewValue value)
        {
            LiteDatabase db = getLiteDatabase();//get database connection
            try
            {
                using (db)
                {
                    var collection = db.GetCollection<Product>("Product");

                    Product dbProduct = collection.FindById(id);//find current product
                    if (ReferenceEquals(null, dbProduct))
                    {
                        return;//if we dont find one then nothing to update
                    }
                    dbProduct.Prices[0].Value = double.Parse(value.value);//set value

                    collection.Update(dbProduct);//insert to db
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public class NewValue
        {
            public string value { get; set; }
        }
    }
}
