namespace GrupoExito.Entities.Responses.Recipes
{
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class RecipeCategoryResponse : ResponseBase
    {
        public RecipeCategoryResponse()
        {
            Categories = new List<RecipeCategory>();
        }

        public IList<RecipeCategory> Categories { get; set; }

        [JsonProperty("dayRecipe")]
        public RecipeOfDay Recipe { get; set; }
    }
}
