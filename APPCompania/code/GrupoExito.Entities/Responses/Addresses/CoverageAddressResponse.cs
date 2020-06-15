namespace GrupoExito.Entities.Responses.Addresses
{
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;

    public class CoverageAddressResponse : ResponseBase
    {
        [JsonProperty("standardAddress")]
        public string StandardAddress { get; set; }

        [JsonProperty("idDependency")]
        public string DependencyId { get; set; }

        [JsonProperty("neighborhood")]
        public string Neighborhood { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("longitude")]
        public decimal Longitude { get; set; }

        [JsonProperty("latitude")]
        public decimal Latitude { get; set; }        
    }
}
