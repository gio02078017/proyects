namespace GrupoExito.Entities.Responses.Products
{
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class ProductsShoppingListResponse : ResponseBase
    {
        public ProductsShoppingListResponse()
        {
            this.ProductsClient = new List<Product>();
        }

        [JsonProperty("name")]
        public string Name { get; set; }
        public int SequenceNum { get; set; }

        [JsonProperty("listId")]
        public string ListId { get; set; }

        [JsonProperty("productsClient")]
        public IList<Product> ProductsClient { get; set; }
    }
}
