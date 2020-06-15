namespace GrupoExito.Entities.Entiites
{
    using GrupoExito.Entities.Entiites.Products;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class ShoppingList
    {
        public ShoppingList()
        {
            this.Products = new List<ProductList>();
        }

        public string Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string SiteId { get; set; }

        [JsonProperty("countItems")]
        public string QuantityProducts { get; set; }
        public IList<ProductList> Products { get; set; }
    }
}
