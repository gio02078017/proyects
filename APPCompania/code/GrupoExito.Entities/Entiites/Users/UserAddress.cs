namespace GrupoExito.Entities
{
    using Newtonsoft.Json;

    public class UserAddress
    {
        [JsonProperty("addressKey")]
        public string AddressKey { get; set; }

        [JsonProperty("address")]
        public string AddressComplete { get; set; }

        public string City { get; set; }

        [JsonProperty("cityId")]
        public string CityId { get; set; }

        [JsonProperty("description")]
        public string Description  { get; set; }

        [JsonProperty("departmentId")]
        public string StateId { get; set; }

        [JsonProperty("aditionalInformationAddress")]
        public string AditionalInformationAddress { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("telephone")]
        public string CellPhone { get; set; }

        [JsonProperty("isDefaultAddress")]
        public bool IsDefaultAddress { get; set; }

        [JsonProperty("idDependency")]
        public string DependencyId { get; set; }

        public string Neighborhood { get; set; }

        public string Location { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public bool HasCoverage { get; set; }       

        public string AddressName { get; set; }

        public string IdPickup { get; set; }

        [JsonProperty("regionId")]
        public string Region { get; set; }

        public string SelectedAddress { get; set; }
    }
}
