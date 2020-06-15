namespace GrupoExito.Entities.Responses.Products
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class CategoriesResponse : ResponseBase
    {
        public CategoriesResponse()
        {
            Categories = new List<Category>();
        }

        [JsonProperty("productCategories")]
        public IList<Category> Categories { get; set; }
    }
}
