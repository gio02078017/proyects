namespace GrupoExito.Entities
{
    using Newtonsoft.Json;

    public class CitiesFilter
    {
        [JsonProperty("homeDelivery")]
        public string HomeDelivery { get; set; }

        [JsonProperty("pickup")]
        public string Pickup { get; set; }
    }
}
