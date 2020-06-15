namespace GrupoExito.Entities.Responses.Products
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class ProductsResponse : ProductResponseBase
    {
        public ProductsResponse()
        {
            this.Products = new List<Product>();
            this.Brands = new List<ProductFilter>();
            this.Categories = new List<ProductFilter>();
        }
        
        [JsonProperty("products")]
        public List<Product> Products { get; set; }

        [JsonProperty("brands")]
        public IList<ProductFilter> Brands { get; set; }

        [JsonProperty("categories")]
        public IList<ProductFilter> Categories { get; set; }

        [JsonProperty("totalProductsSearch")]
        public int TotalProductsSearch { get; set; }
    }
}
