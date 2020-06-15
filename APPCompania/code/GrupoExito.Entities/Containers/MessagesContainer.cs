namespace GrupoExito.Entities.Containers
{
    using GrupoExito.Entities;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class MessagesContainer
    {
        [JsonProperty("hasErrors")]
        public bool HasErrors { get; set; }
        [JsonProperty("listMessages")]
        public List<MessageAplication> Messages { get; set; }
    }
}
