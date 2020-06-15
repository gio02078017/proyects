namespace GrupoExito.Entities.Responses.Base
{
    using GrupoExito.Entities.Containers;
    using Newtonsoft.Json;
    using System.Net.Http;

    public class ResponseBase
    {
        [JsonIgnore()]
        public HttpResponseMessage HttpResponse { get; set; }

        [JsonProperty("messages")]
        public MessagesContainer Result { get; set; }      
    }
}
