namespace GrupoExito.Entities.Entiites
{
    using Newtonsoft.Json;

    public class RecipeOfDay
    {
        [JsonProperty("idContent")]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Subtitle { get; set; }
        public string Ingredients { get; set; }
        public string Preparation { get; set; }
        public string PreparationTime { get; set; }
        public string Difficulty { get; set; }
    }
}
