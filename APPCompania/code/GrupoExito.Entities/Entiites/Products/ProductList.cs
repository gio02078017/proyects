namespace GrupoExito.Entities.Entiites.Products
{
    using Newtonsoft.Json;

    public class ProductList : Product
    {
        [JsonProperty("itemId")]
        public string ItemId { get; set; }

        /// Shopping list
        [JsonProperty("listId")]
        public string ShoppingListId { get; set; }
    }
}
