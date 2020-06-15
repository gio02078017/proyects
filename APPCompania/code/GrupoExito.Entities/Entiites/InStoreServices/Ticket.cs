namespace GrupoExito.Entities.Entiites
{
    using Newtonsoft.Json;

    public class Ticket
    {
        [JsonProperty("branch_id")]
        public string  StoreId { get; set; }

        [JsonProperty("mobile_id")]
        public string MobileId { get; set; }

        [JsonProperty("ticket_name")]
        public string Name { get; set; }

        [JsonProperty("ticket_id")]
        public string Id { get; set; }

        public int WaitingTickets { get; set; }

        public decimal AvgWaitTime { get; set; }

        public decimal AvgWaitServ { get; set; }

        public bool Success { get; set; }
    }
}
