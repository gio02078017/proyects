namespace GrupoExito.Entities
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class Summary
    {
        public Summary()
        {
            Products = new List<Product>();
        }

        [JsonProperty("orderId")]
        public string Id { get; set; }

        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [JsonProperty("dependencyId")]
        public string DependencyId { get; set; }

        [JsonProperty("subTotal")]
        public decimal SubTotal { get; set; }

        [JsonProperty("total")]
        public decimal Total { get; set; }

        [JsonProperty("tax")]
        public decimal Tax { get; set; }

        [JsonProperty("countryTax")]
        public decimal CountryTax { get; set; }

        [JsonProperty("countyTax")]
        public decimal CountyTax { get; set; }

        [JsonProperty("amountPoints")]
        public decimal AmountPoints { get; set; }

        [JsonProperty("totalProducts")]
        public int TotalProducts { get; set; }

        [JsonProperty("discounts")]
        public decimal Discounts { get; set; }

        public IList<Product> Products { get; set; }
    }
}
