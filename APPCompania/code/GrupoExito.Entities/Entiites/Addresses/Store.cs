namespace GrupoExito.Entities.Entiites
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class Store
    {
        public Store()
        {
            Type = new DependencyType();
            Services = new List<DependencyService>();
        }

        [JsonProperty("dependencyId")]
        public string Id { get; set; }

        [JsonProperty("id")]
        public string StoreId { get; set; }

        [JsonProperty("idMark")]
        public string IdMark { get; set; }

        [JsonProperty("idDependencyType")]
        public int IdDependencyType { get; set; }

        [JsonProperty("dependencyName")]
        public string Name { get; set; }

        [JsonProperty("name")]
        public string DependencyName { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("express")]
        public bool Express { get; set; }

        public string CityId { get; set; }

        [JsonProperty("dependencyIdPickup")]
        public string IdPickup { get; set; }

        public string Schedules { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        [JsonProperty("is24Hours")]
        public bool OpenAllDay { get; set; }

        [JsonProperty("dependencyType")]
        public DependencyType Type { get; set; }

        [JsonProperty("services")]
        public IList<DependencyService> Services { get; set; }

        [JsonProperty("phone")]
        public string PhoneNumber { get; set; }

        public string Region { get; set; }

        public string City { get; set; }
    }
}
