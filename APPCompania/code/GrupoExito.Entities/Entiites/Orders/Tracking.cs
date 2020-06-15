namespace GrupoExito.Entities.Entiites
{
    using Newtonsoft.Json;
    using System;

    public class Tracking
    {
        [JsonProperty("dateState")]
        public string Date { get; set; }

        [JsonProperty("nameState")]
        public string StatusName { get; set; }

        [JsonProperty("state")]
        public bool Status { get; set; }
    }
}
