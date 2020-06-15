namespace GrupoExito.Entities.Responses.Orders
{
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;

    public class SummaryResponse : ResponseBase
    {
        [JsonProperty("order")]
        public Summary Summary { get; set; }
    }
}
