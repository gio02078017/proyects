namespace GrupoExito.Entities.Entiites
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class RecipeDetail
    {
        public RecipeDetail()
        {
            PreparationSteps = new List<string>();
            Ingredients = new List<string>();
        }

        public string Id { get; set; }
        [JsonProperty("imageMiniature")]
        public string ShortImage { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Subtitle { get; set; }

        [JsonProperty("descriptionContent")]
        public string Ingredient { get; set; }

        [JsonProperty("termnsConditions")]
        public string Preparation { get; set; }

        [JsonProperty("link")]
        public string PreparationTime { get; set; }

        [JsonProperty("socialNetworkLink")]
        public string Difficulty { get; set; }

        public IList<string> PreparationSteps { get; set; }

        public IList<string> Ingredients { get; set; }

    }
}
