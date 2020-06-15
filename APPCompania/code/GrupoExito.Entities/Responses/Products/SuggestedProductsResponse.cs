namespace GrupoExito.Entities.Responses.Products
{
    using GrupoExito.Entities.Entiites.Products;
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class SuggestedProductsResponse : ResponseBase
    {
        public SuggestedProductsResponse()
        {
            this.ProductsClient = new List<ProductList>();
        }

        [JsonProperty("name")]
        public string Name { get; set; }
        public int SequenceNum { get; set; }

        [JsonProperty("listId")]
        public string ListId { get; set; }

        [JsonProperty("productsClient")]
        public IList<ProductList> ProductsClient { get; set; }
    }
}
