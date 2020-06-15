namespace GrupoExito.Entities.Responses.InStoreServices
{
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;

    public class DeviceResponse : ResponseBase
    {
        [JsonProperty("mobile_id")]
        public string MobileId { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
