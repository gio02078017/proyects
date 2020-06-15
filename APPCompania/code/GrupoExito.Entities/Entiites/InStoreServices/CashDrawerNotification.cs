namespace GrupoExito.Entities
{
    using Newtonsoft.Json;

    public class CashDrawerNotification
    {
        [JsonProperty("notification")]
        public NotificationBody Body { get; set; }
        [JsonProperty("data")]
        public CashDrawerNotificationData Data { get; set; }
        [JsonProperty("to")]
        public string PushToken { get; set; }
    }
}
