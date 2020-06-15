namespace GrupoExito.Entities.Entiites.Generic.Contents
{
    using Newtonsoft.Json;

    public class Promotion : Content
    {
        [JsonProperty("idPromotion")]
        public string Id { get; set; }
        public string Description { get; set; }
        public string UrlImage { get; set; }

    }
}
