namespace GrupoExito.Entities.Responses.Products
{
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;

    public class ProductResponse : ProductResponseBase
    {
        public ProductResponse()
        {
            this.Product = new Product();
        }

        [JsonProperty("productDetail")]
        public Product Product { get; set; }
    }
}
