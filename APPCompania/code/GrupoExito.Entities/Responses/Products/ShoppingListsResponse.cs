namespace GrupoExito.Entities.Responses.Products
{
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class ShoppingListsResponse : ResponseBase
    {
        public ShoppingListsResponse()
        {
            this.ShpoppingLists = new List<ShoppingList>();
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("lists")]
        public IList<ShoppingList> ShpoppingLists { get; set; } 
    }
}
