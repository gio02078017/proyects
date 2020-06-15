namespace GrupoExito.Entities.Parameters
{
    using Newtonsoft.Json;

    public class ScheduleReservationParameters
    {
        [JsonProperty("idReservation")]
        public string OrderId { get; set; }
        public int QuantityUnits { get; set; }

        [JsonProperty("range")]
        public string Schedule { get; set; }
        [JsonProperty("dependencyCode")]
        public string DependencyId { get; set; }
        public string DeliveryMode { get; set; }
        public string TypeModality { get; set; }
        public string ShippingCost { get; set; }
        public string DateSelected { get; set; }
    }
}
