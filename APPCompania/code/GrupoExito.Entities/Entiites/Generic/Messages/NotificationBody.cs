namespace GrupoExito.Entities
{
    using Newtonsoft.Json;

    public class NotificationBody
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
    }
}
