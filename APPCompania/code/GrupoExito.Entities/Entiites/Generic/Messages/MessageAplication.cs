namespace GrupoExito.Entities
{
    using Newtonsoft.Json;

    public class MessageAplication
    {
        [JsonProperty("isError")]
        public bool IsError { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
