namespace GrupoExito.Entities
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class Product
    {
        public Product()
        {
            this.Price = new Price();
            this.UrlImagesDefault = new List<string>();
            this.UrlImagesXL = new List<string>();
        }

        [JsonProperty("skuId")]
        public string SkuId { get; set; }

        [JsonProperty("prdId")]
        public string Id { get; set; } 

        [JsonProperty("CommerceItemId")]
        public string CommerceItemId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("brand")]
        public string Brand { get; set; }

        [JsonProperty("presentation")]
        public string Presentation { get; set; }

        [JsonProperty("urlMediumImage")]
        public string UrlMediumImage { get; set; }

        [JsonProperty("productType")]
        public string ProductType { get; set; }

        [JsonProperty("stock")]
        public string Stock { get; set; }

        [JsonProperty("price")]
        public Price Price { get; set; }

        [JsonProperty("urlImagesDefault")]
        public IList<string> UrlImagesDefault { get; set; }

        [JsonProperty("urlImagesXL")]
        public IList<string> UrlImagesXL { get; set; }

        [JsonProperty("urlImageNutritionFact")]
        public string UrlImageNutritionFact { get; set; }

        [JsonProperty("weight")]
        public decimal Weight { get; set; }

        [JsonProperty("weightUnits")]
        public string WeightUnits { get; set; }

        [JsonProperty("isEstimatedWeight")]
        public bool IsEstimatedWeight { get; set; }

        [JsonProperty("features")]
        public string Features { get; set; }

        public bool IsLoading { get; set; }
        public bool WeightSelected { get; set; }

        [JsonProperty("categoryId")]
        public string CategoryId { get; set; }

        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }

        [JsonProperty("imageCategory")]
        public string CategoryImage { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("selected")]
        public bool Selected { get; set; }

        public string SalePrice { get; set; }

        /// Parameters for View
        public string SiteId { get; set; }

        public string FactorPum { get; set; }

        [JsonProperty("unidadPum")]
        public string UnityPum { get; set; }

        public decimal PriceProduct { get; set; }
    }
}
