namespace GrupoExito.Entities
{
    using Newtonsoft.Json;

    public class Price 
    {
        [JsonProperty("skuId")]
        public string SkuId { get; set; }

        [JsonProperty("priceNow")]
        public decimal ActualPrice { get; set; }

        [JsonProperty("priceBefore")]
        public decimal PreviousPrice { get; set; }

        [JsonProperty("percentDiscount")]
        public decimal DiscountPercent { get; set; }

        [JsonProperty("UrlIcon")]
        public string DiscountImage { get; set; }

        [JsonProperty("priceOtherMeans")]
        public decimal PriceOtherMeans { get; set; }

        [JsonProperty("urlStar")]
        public string Image { get; set; }

        [JsonProperty("infoPum")]
        public string Pum { get; set; }

    }
}
