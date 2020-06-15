namespace GrupoExito.Entities.Entiites
{
    using Newtonsoft.Json;

    public class Recipe
    {
        public string Id { get; set; }
        [JsonProperty("imageMiniature")]
        public string ShortImage { get; set; }

        [JsonProperty("name")]
        public string Title { get; set; }
        public string Image { get; set; }
        public string Subtitle { get; set; }

        [JsonProperty("descriptionContent")]
        public string Ingredients { get; set; }

        [JsonProperty("termnsConditions")]
        public string Preparation { get; set; }

        public string PreparationTime { get; set; }

        public string Difficulty { get; set; }
    }
}
