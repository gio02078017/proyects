namespace GrupoExito.Entities.Entiites
{
    using Newtonsoft.Json;

    public class AppDevice
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("push_token")]
        public string TokenNotificationPush { get; set; }
    }
}
