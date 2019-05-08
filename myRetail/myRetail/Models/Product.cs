using System.Collections.Generic;

namespace myRetail.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Price> Prices { get; set; }
    }

    public class Price
    {
        public double Value { get; set; }
        public CurrencyCodeTypes CurrencyCode { get; set; }
    }

    public enum CurrencyCodeTypes { USD, EUR, AOA, XCD }
}