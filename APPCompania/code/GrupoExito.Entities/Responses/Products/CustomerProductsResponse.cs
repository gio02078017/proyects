namespace GrupoExito.Entities.Responses.Products
{
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class CustomerProductsResponse : ProductResponseBase
    {
        public CustomerProductsResponse()
        {
            this.CustomerProducts = new List<Product>();
            this.Plus = new List<string>();
        }

        [JsonProperty("productsClient")]
        public IList<Product> CustomerProducts { get; set; }

        public string NextDateCache { get; set; }
        public List<string> Plus { get; set; }
    }
}
