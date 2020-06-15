namespace GrupoExito.Entities.Responses.Base
{
    using Newtonsoft.Json;

    public class ProductResponseBase : ResponseBase
    {
        [JsonProperty("totalProducts")]
        public int TotalProducts { get; set; }

        [JsonProperty("subTotal")]
        public decimal SubTotal { get; set; }

        [JsonProperty("total")]
        public decimal Total { get; set; }

        [JsonProperty("countyTax")]
        public decimal CountyTax { get; set; }

        [JsonProperty("countryTax")]
        public decimal CountryTax { get; set; }

        [JsonProperty("discounts")]
        public decimal Discounts { get; set; }
    }
}
