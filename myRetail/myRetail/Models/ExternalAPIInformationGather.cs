using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace myRetail.Models
{
    public class ExternalAPIInformationGather
    {
        /// <summary>
        /// Grab URL from configuration file and save it for reuse.
        /// </summary>
        private string Url;
        public string getUrl()
        {
            if (ReferenceEquals(null, Url))
            {
                try
                {
                    Url = ConfigurationManager.AppSettings["targetUrl"];
                    return Url;
                }
                catch (ConfigurationErrorsException ex)
                {
                    throw new Exception("Missing the url for Target's Products. " + ex.Message);
                }
            }
            return Url;
        }

        /// <summary>
        /// Grab URL parameters from configuration file and save it for reuse.
        /// </summary>
        private string UrlParameters;
        public string getUrlParameters()
        {
            if (ReferenceEquals(null, UrlParameters))
            {
                try
                {
                    UrlParameters = ConfigurationManager.AppSettings["targetUrlParams"];
                    return UrlParameters;
                }
                catch (ConfigurationErrorsException ex)
                {
                    throw new Exception("Missing the url params for Target's Products. " + ex.Message);
                }
            }
            return UrlParameters;
        }

        /// <summary>
        /// Gets name using dynamics from a returned json string.
        /// </summary>
        /// <param name="id">ID to search.</param>
        /// <returns>The name of the product or null if not found.</returns>
        public string GetName(int id)
        {
            try
            {
                dynamic results = JsonConvert.DeserializeObject<dynamic>(GetResponse(id).Result);
                var products = results.product.item.product_description.title;//structure grabbed from postman response
                return (string)products;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets price using dynamics from a returned json string.
        /// </summary>
        /// <param name="id">ID to search.</param>
        /// <returns>The price of the product or null if not found.</returns>
        public string GetInitialPrice(int id)
        {
            try
            {
                dynamic results = JsonConvert.DeserializeObject<dynamic>(GetResponse(id).Result);
                var products = results.product.price.listPrice.price;//structure grabbed from postman response
                return (string)products;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Calls the rest service and retrieves the products data.
        /// </summary>
        /// <param name="id">ID to search.</param>
        /// <returns>The string body of the response.</returns>
        private async Task<string> GetResponse(int id)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(getUrl() + id.ToString());
                client.Timeout = new TimeSpan(0,0,10);                

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                // List data response.
                HttpResponseMessage response = await client.GetAsync(getUrlParameters()).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();//read body

                client.Dispose();//dispose of open client

                return responseBody;
            }
            catch (Exception ex)
            {
                throw ex;//throw exception so we can return null in calling methods.
            }
        }
    }
}