namespace GrupoExito.Entities.Entiites
{
    using Newtonsoft.Json;

    public class ShippingMethod
    {
        [JsonProperty("telephone")]
        public string CellPhone { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }
    }
}
