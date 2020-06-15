namespace GrupoExito.Entities
{
    using Newtonsoft.Json;

    public class ProductParameters
    {
        [JsonProperty("skuId")]
        public string SkuId { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("dependencyId")]
        public string DependencyId { get; set; }

        [JsonProperty("productType")]
        public string ProductType { get; set; }

        public string OrderId { get; set; }
        public string Size { get; set; }
        public string From { get; set; }
    }
}
