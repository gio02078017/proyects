namespace GrupoExito.Entities.Responses.Recipes
{
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class RecipeResponse : ResponseBase
    {
        public RecipeResponse()
        {
            Recipe = new List<RecipeDetail>();
        }

        [JsonProperty("contents")]
        public IList<RecipeDetail> Recipe { get; set; }
    }
}
