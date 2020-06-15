namespace GrupoExito.Entities.Responses.Recipes
{
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class RecipesResponse : ResponseBase
    {
        public RecipesResponse()
        {
            Recipes = new List<Recipe>();
        }

        [JsonProperty("iconCategory")]
        public string CategoryImage { get; set; }

        [JsonProperty("nameCategory")]
        public string CategoryName { get; set; }

        [JsonProperty("recipes")]
        public IList<Recipe> Recipes { get; set; }
    }
}
