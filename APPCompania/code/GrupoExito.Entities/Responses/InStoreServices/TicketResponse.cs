namespace GrupoExito.Entities.Responses.InStoreServices
{
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;

    public class TicketResponse : ResponseBase
    {
        [JsonProperty("ticket_name")]
        public string Name { get; set; }

        [JsonProperty("ticket_id")]
        public string Id { get; set; }

        public int WaitingTickets { get; set; }

        public decimal AvgWaitTime { get; set; }

        public decimal AvgWaitServ { get; set; }

        public bool Success { get; set; }

        [JsonProperty("ticket_status")]
        public string Status { get; set; }

        [JsonProperty("tickets_in_front")]
        public int TicketInFront { get; set; }

        [JsonProperty("wait_estimate")]
        public int WaitEstimate { get; set; }

        [JsonProperty("slot_display_name")]
        public string SlotDisplayName { get; set; }
    }
}
