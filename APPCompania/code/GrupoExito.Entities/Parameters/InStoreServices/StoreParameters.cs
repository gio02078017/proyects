using Newtonsoft.Json;

namespace GrupoExito.Entities.Parameters
{
    public class StoreParameters
    {
        [JsonProperty("cityId")]
        public string CityId { get; set; }

        [JsonProperty("pickUp")]
        public bool PickUp { get; set; }

        [JsonProperty("homeDelivery")]
        public bool HomeDelivery { get; set; }

        [JsonProperty("cityIdPickup")]
        public string IdPickup { get; set; }
    }
}
