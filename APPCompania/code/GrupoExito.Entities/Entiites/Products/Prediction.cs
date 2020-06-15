namespace GrupoExito.Entities
{
    using Newtonsoft.Json;

    public class Prediction
    {
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
