namespace GrupoExito.Entities.Entiites
{
    using Newtonsoft.Json;

    public class ContentImage
    {       
        [JsonProperty("urlImage")]
        public string Image { get; set; }
    }
}
