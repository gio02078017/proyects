namespace GrupoExito.Entities
{
    using Newtonsoft.Json;
    using System;

    public class AppNotification
    {
        [JsonProperty("idNotification")]
        public int IdNotification { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("idContentType")]
        public int? IdContentType { get; set; }

        [JsonProperty("idContent")]
        public int? IdContent { get; set; }

        [JsonProperty("dateExpire")]
        public DateTime? DateExpire { get; set; }

        [JsonProperty("idMark")]
        public int IdMark { get; set; }

        [JsonProperty("publish")]
        public bool Publish { get; set; }

        [JsonProperty("readed")]
        public bool Readed { get; set; }
    }
}
