namespace GrupoExito.Entities.Entiites
{
    using Newtonsoft.Json;

    public class RecipeCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string Image { get; set; }
    }
}
