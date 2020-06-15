namespace GrupoExito.Entities
{
    using Newtonsoft.Json;

    public class City
    {
        [JsonProperty("cityId")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("departmentId")]
        public string State { get; set; }

        [JsonProperty("regionId")]
        public string Region { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [JsonProperty("cityIdPickup")]
        public string IdPickup { get; set; }
    }
}
