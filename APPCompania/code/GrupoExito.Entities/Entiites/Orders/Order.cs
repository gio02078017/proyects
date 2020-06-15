namespace GrupoExito.Entities
{
    using GrupoExito.Entities.Entiites.Payments;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class Order : OrderPay
    {
        public Order()
        {
            Products = new List<Product>();
        }

        [JsonProperty("orderId")]
        public string Id { get; set; }

        [JsonProperty("dependencyId")]
        public string DependencyId { get; set; }

        [JsonProperty("subTotal")]
        public decimal SubTotal { get; set; }

        [JsonProperty("totalProducts")]
        public int TotalProducts { get; set; }

        [JsonProperty("countyTax")]
        public decimal CountyTax { get; set; }

        [JsonProperty("countryTax")]
        public decimal CountryTax { get; set; }

        [JsonProperty("discounts")]
        public decimal Discounts { get; set; }

        [JsonProperty("total")]
        public decimal Total { get; set; }

        [JsonProperty("state")]
        public string Status { get; set; }

        public string Date { get; set; }

        public IList<Product> Products { get; set; }        
    }
}
