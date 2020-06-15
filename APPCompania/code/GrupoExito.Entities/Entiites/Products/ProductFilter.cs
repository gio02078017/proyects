namespace GrupoExito.Entities
{
    using Newtonsoft.Json;

    public class ProductFilter
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("count")]
        public string Quantity { get; set; }

        public bool Checked { get; set; }
    }
}
