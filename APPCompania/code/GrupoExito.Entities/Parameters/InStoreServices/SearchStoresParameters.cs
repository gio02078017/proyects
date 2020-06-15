namespace GrupoExito.Entities.Parameters
{
    using Newtonsoft.Json;

    public class SearchStoresParameters
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }
}
