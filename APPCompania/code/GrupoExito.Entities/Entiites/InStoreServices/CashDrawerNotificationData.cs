namespace GrupoExito.Entities
{
    using Newtonsoft.Json;

    public class CashDrawerNotificationData
    {
        [JsonProperty("ticket_name")]
        public string TicketName { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("action")]
        public string Action { get; set; }
        [JsonProperty("msg")]
        public string Message { get; set; }
        [JsonProperty("slot_display_name")]
        public string BoxNumber { get; set; }
    }
}
