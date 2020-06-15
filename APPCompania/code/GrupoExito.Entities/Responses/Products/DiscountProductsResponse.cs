namespace GrupoExito.Entities.Responses.Products
{
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class DiscountProductsResponse : ProductResponseBase
    {
        public DiscountProductsResponse()
        {
            this.DiscountProducts = new List<Product>();
        }

        [JsonProperty("productsAppDiscounts")]
        public IList<Product> DiscountProducts { get; set; }
    }
}
